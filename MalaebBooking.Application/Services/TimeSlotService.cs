using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.TimeSlots;
using MalaebBooking.Application.Errors;
using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using Mapster;

namespace MalaebBooking.Application.Services;

public class TimeSlotService : ITimeSlotService
{
    private readonly ITimeSlotRepository _timeSlotRepository;

    public TimeSlotService(ITimeSlotRepository timeSlotRepository)
    {
        _timeSlotRepository = timeSlotRepository;
    }

    public async Task<Result<TimeSlotResponse>> CreateTimeSlotAsync(CreateTimeSlotRequest request)
    {
        // تحقق من صحة الوقت (البداية قبل النهاية)
        if (request.StartTime >= request.EndTime)
        {
            return Result.Failure<TimeSlotResponse>(TimeSlotErrors.StartTimeAfterEndTime);
        }

        var timeSlot = request.Adapt<TimeSlot>();

        var hasConflict = await _timeSlotRepository.HasOverlappingSlotsAsync(
            timeSlot.StadiumId,
            timeSlot.Date,
            timeSlot.StartTime,
            timeSlot.EndTime
        );

        if (hasConflict)
            return Result.Failure<TimeSlotResponse>(TimeSlotErrors.Overlapping);

        await _timeSlotRepository.AddAsync(timeSlot);

        var response = timeSlot.Adapt<TimeSlotResponse>();

        return Result.Success(response);
    }

    public async Task<Result> DeleteTimeSlotAsync(int id)
    {
        var timeslot = await _timeSlotRepository.GetByIdAsync(id);

        if (timeslot == null)
            return Result.Failure(TimeSlotErrors.NotFound);

        await _timeSlotRepository.DeleteAsync(timeslot);

        return Result.Success();
    }

    public async Task<Result<IEnumerable<TimeSlotResponse>>> GetAllByStadiumAsync(int stadiumId)
    {
        var timeslots = await _timeSlotRepository.GetAllByStadiumIdAsync(stadiumId);
        
        // Removed the null check since an empty list is a valid response, not an error.
        var response = timeslots.Adapt<IEnumerable<TimeSlotResponse>>();

        return Result.Success(response);
    }

    public async Task<Result<IEnumerable<TimeSlotResponse>>> GetAvailableByStadiumAndDateAsync(int stadiumId, DateOnly date)
    {
        var timeslots = await _timeSlotRepository.GetAvailableByStadiumAndDateAsync(stadiumId, date);

        var response = timeslots.Adapt<IEnumerable<TimeSlotResponse>>();

        return Result.Success(response);
    }

    public async Task<Result<TimeSlotResponse>> GetByIdAsync(int id)
    {
        var timeslot = await _timeSlotRepository.GetByIdAsync(id);
        
        if (timeslot == null)
            return Result.Failure<TimeSlotResponse>(TimeSlotErrors.NotFound);
            
        var response = timeslot.Adapt<TimeSlotResponse>();
        
        return Result.Success(response);
    }

    public async Task<Result<IEnumerable<TimeSlotResponse>>> GetByStadiumAndDateAsync(int stadiumId, DateOnly date)
    {
        var timeslots = await _timeSlotRepository.GetByStadiumAndDateAsync(stadiumId, date);
        var response = timeslots.Adapt<IEnumerable<TimeSlotResponse>>();
        
        return Result.Success(response);
    }

    public async Task<Result<bool>> HasOverlappingSlotsAsync(int stadiumId, DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        var hasOverlap = await _timeSlotRepository.HasOverlappingSlotsAsync(stadiumId, date, startTime, endTime);
        
        return Result.Success(hasOverlap);
    }

    public async Task<Result<bool>> IsSlotAvailableAsync(int slotId)
    {
        var isAvailable = await _timeSlotRepository.IsSlotAvailableAsync(slotId);
        
        return Result.Success(isAvailable);
    }

    public async Task<Result<TimeSlotResponse>> UpdateTimeSlotAsync(int id, UpdateTimeSlotRequest request)
    {
        var existing = await _timeSlotRepository.GetByIdAsync(id);
        
        if (existing == null)
            return Result.Failure<TimeSlotResponse>(TimeSlotErrors.NotFound);

        // Map request to existing entity properties. Mapster handles this elegantly.
        request.Adapt(existing);
        
        // Since we mapped the Id from route differently, ensure it's preserved
        existing.Id = id;

        await _timeSlotRepository.UpdateAsync(existing);

        var response = existing.Adapt<TimeSlotResponse>();

        return Result.Success(response);
    }
}
