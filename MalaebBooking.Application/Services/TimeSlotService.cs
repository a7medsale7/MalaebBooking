using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.TimeSlots;
using MalaebBooking.Application.Errors;
using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using MalaebBooking.Domain.Enums;
using Mapster;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Services;

public class TimeSlotService : ITimeSlotService
{
    private readonly ITimeSlotRepository _timeSlotRepository;
    private readonly IStadiumRepository _stadiumRepository;
    private readonly IScheduleRuleRepository _scheduleRuleRepository;

    public TimeSlotService(
        ITimeSlotRepository timeSlotRepository,
        IStadiumRepository stadiumRepository,
        IScheduleRuleRepository scheduleRuleRepository)
    {
        _timeSlotRepository = timeSlotRepository;
        _stadiumRepository = stadiumRepository;
        _scheduleRuleRepository = scheduleRuleRepository;
    }

    // ==========================================
    // 1. معاينة المواعيد (تقطيع البلوك לסاعات)
    // ==========================================
    public async Task<Result<IEnumerable<PreviewTimeSlotResponse>>> GeneratePreviewAsync(PreviewTimeSlotsRequest request)
    {
        // 1. التأكد من منطقية الوقت
        if (request.RangeStart >= request.RangeEnd)
            return Result.Failure<IEnumerable<PreviewTimeSlotResponse>>(TimeSlotErrors.InvalidTimeRange);

        // 2. نجيب الملعب
        var stadium = await _stadiumRepository.GetByIdAsync(request.StadiumId);
        if (stadium is null)
            return Result.Failure<IEnumerable<PreviewTimeSlotResponse>>(TimeSlotErrors.StadiumNotFound);

        var previewSlots = new List<PreviewTimeSlotResponse>();
        var currentTime = request.RangeStart;

        // 3. لوب تقطيع الوقت
        while (currentTime < request.RangeEnd)
        {
            // حساب وقت نهاية الـ Slot بناءً على مدة الحجز الخاصة بالملعب
            var nextTime = currentTime.AddMinutes(stadium.SlotDurationMinutes);
            if (nextTime > request.RangeEnd) break;

            // هل الساعة دي موجودة في الداتا بيز قبل كده؟
            var isExists = await _timeSlotRepository.HasOverlappingSlotsAsync(
                request.StadiumId, request.Date, currentTime, nextTime);

            // حدد الموسم وبناءً عليه حدد بداية ليل الملعب ده
            int month = request.Date.Month;
            bool isSummer = month >= 5 && month <= 10;
            TimeOnly actualNightStartTime = isSummer ? stadium.SummerNightStartTime : stadium.WinterNightStartTime;

            // هل الميعاد الحالي بالليل ولا الصبح؟
            bool isNightTime = currentTime >= actualNightStartTime;

            // حساب السعر النهائي
            decimal calculatedPrice = (decimal)(nextTime - currentTime).TotalHours *
                                      (isNightTime ? stadium.PricePerHourNight : stadium.PricePerHourDay);

            // إضافة الساعة بعد حساب كل تفاصيلها للستة
            previewSlots.Add(new PreviewTimeSlotResponse
            {
                StartTime = currentTime,
                EndTime = nextTime,
                Price = calculatedPrice,
                IsAlreadyExists = isExists
            });

            // تحديث الوقت عشان ننقل على الساعة اللي بعدها (مهم جداً عشان ميعملش Infinite Loop)
            currentTime = nextTime;
        }

        return Result.Success<IEnumerable<PreviewTimeSlotResponse>>(previewSlots);
    }


    // ==========================================
    // 2. حفظ المواعيد ككتلة واحدة وإنشاء Rule
    // ==========================================
    public async Task<Result> BulkCreateAsync(BulkCreateTimeSlotsRequest request)
    {
        var stadium = await _stadiumRepository.GetByIdAsync(request.StadiumId);
        if (stadium == null)
            return Result.Failure(TimeSlotErrors.StadiumNotFound);

        int? createdRuleId = null;

        if (request.IsRecurring && request.SelectedSlots.Any())
        {
            var startOfBlock = request.SelectedSlots.Min(s => s.StartTime);
            var endOfBlock = request.SelectedSlots.Max(s => s.EndTime);

            var newRule = new ScheduleRule
            {
                StadiumId = request.StadiumId,
                StartTime = startOfBlock,
                EndTime = endOfBlock,
                IsActive = true
            };

            await _scheduleRuleRepository.AddAsync(newRule);
            createdRuleId = newRule.Id;
        }

        var slotsToCreate = new List<TimeSlot>();

        foreach (var selected in request.SelectedSlots)
        {
            var hasConflict = await _timeSlotRepository.HasOverlappingSlotsAsync(
                request.StadiumId, request.Date, selected.StartTime, selected.EndTime);

            if (!hasConflict)
            {
                slotsToCreate.Add(new TimeSlot
                {
                    StadiumId = request.StadiumId,
                    Date = request.Date,
                    StartTime = selected.StartTime,
                    EndTime = selected.EndTime,
                    Status = SlotStatus.Available,
                    ScheduleRuleId = createdRuleId
                });
            }
        }

        if (slotsToCreate.Any())
        {
            await _timeSlotRepository.AddRangeAsync(slotsToCreate);
        }

        return Result.Success();
    }

    // =========================================================
    // 3. كل الـ Methods العادية
    // =========================================================
    public async Task<Result<TimeSlotResponse>> CreateTimeSlotAsync(CreateTimeSlotRequest request)
    {
        if (request.StartTime >= request.EndTime)
            return Result.Failure<TimeSlotResponse>(TimeSlotErrors.StartTimeAfterEndTime);

        var timeSlot = request.Adapt<TimeSlot>();

        var hasConflict = await _timeSlotRepository.HasOverlappingSlotsAsync(
            timeSlot.StadiumId, timeSlot.Date, timeSlot.StartTime, timeSlot.EndTime);

        if (hasConflict)
            return Result.Failure<TimeSlotResponse>(TimeSlotErrors.Overlapping);

        await _timeSlotRepository.AddAsync(timeSlot);

        var stadium = await _stadiumRepository.GetByIdAsync(timeSlot.StadiumId);
        var response = timeSlot.Adapt<TimeSlotResponse>();

        if (stadium != null)
        {
            // 1. شوف الميعاد ده في شهر کام عشان نعرف إحنا صيف ولا شتا
            int month = timeSlot.Date.Month;
            bool isSummer = month >= 5 && month <= 10; // من شهر 5 لشهر 10 ده صيف

            // 2. هات بداية الليل بتاعت الموسم الصح
            TimeOnly actualNightStartTime = isSummer ? stadium.SummerNightStartTime : stadium.WinterNightStartTime;

            // 3. قارن وقت الحجز ببداية الليل
            bool isNightTime = timeSlot.StartTime >= actualNightStartTime;

            // 4. احسب السعر
            response.Price = (decimal)(timeSlot.EndTime - timeSlot.StartTime).TotalHours *
                             (isNightTime ? stadium.PricePerHourNight : stadium.PricePerHourDay);
        }

        return Result.Success(response);
    }

    public async Task<Result> DeleteTimeSlotAsync(int id)
    {
        var timeslot = await _timeSlotRepository.GetByIdAsync(id);
        if (timeslot == null) return Result.Failure(TimeSlotErrors.NotFound);
        await _timeSlotRepository.DeleteAsync(timeslot);
        return Result.Success();
    }

    public async Task<Result<IEnumerable<TimeSlotResponse>>> GetAllByStadiumAsync(int stadiumId)
    {
        var timeslots = await _timeSlotRepository.GetAllByStadiumIdAsync(stadiumId);
        return Result.Success(timeslots.Adapt<IEnumerable<TimeSlotResponse>>());
    }

    public async Task<Result<IEnumerable<TimeSlotResponse>>> GetAvailableByStadiumAndDateAsync(int stadiumId, DateOnly date)
    {
        var timeslots = await _timeSlotRepository.GetAvailableByStadiumAndDateAsync(stadiumId, date);
        return Result.Success(timeslots.Adapt<IEnumerable<TimeSlotResponse>>());
    }

    public async Task<Result<TimeSlotResponse>> GetByIdAsync(int id)
    {
        var timeslot = await _timeSlotRepository.GetByIdAsync(id);
        if (timeslot == null) return Result.Failure<TimeSlotResponse>(TimeSlotErrors.NotFound);
        return Result.Success(timeslot.Adapt<TimeSlotResponse>());
    }

    public async Task<Result<IEnumerable<TimeSlotResponse>>> GetByStadiumAndDateAsync(int stadiumId, DateOnly date)
    {
        var timeslots = await _timeSlotRepository.GetByStadiumAndDateAsync(stadiumId, date);
        return Result.Success(timeslots.Adapt<IEnumerable<TimeSlotResponse>>());
    }

    public async Task<Result<bool>> HasOverlappingSlotsAsync(int stadiumId, DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        return Result.Success(await _timeSlotRepository.HasOverlappingSlotsAsync(stadiumId, date, startTime, endTime));
    }

    public async Task<Result<bool>> IsSlotAvailableAsync(int slotId)
    {
        return Result.Success(await _timeSlotRepository.IsSlotAvailableAsync(slotId));
    }

    public async Task<Result<TimeSlotResponse>> UpdateTimeSlotAsync(int id, UpdateTimeSlotRequest request)
    {
        var existing = await _timeSlotRepository.GetByIdAsync(id);
        if (existing == null) return Result.Failure<TimeSlotResponse>(TimeSlotErrors.NotFound);

        request.Adapt(existing);
        existing.Id = id;
        await _timeSlotRepository.UpdateAsync(existing);
        return Result.Success(existing.Adapt<TimeSlotResponse>());
    }
}
