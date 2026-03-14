using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.Bookings;
using MalaebBooking.Application.Errors;
using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using MalaebBooking.Domain.Enums;
using Mapster;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MalaebBooking.Application.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ITimeSlotRepository _timeSlotRepository;
    private readonly IStadiumRepository _stadiumRepository; // محتاجينه عشان السعر
    private readonly IEmailSender _emailSender;

    public BookingService(
        IBookingRepository bookingRepository,
        ITimeSlotRepository timeSlotRepository,
        IStadiumRepository stadiumRepository,
        IEmailSender emailSender)
    {
        _bookingRepository = bookingRepository;
        _timeSlotRepository = timeSlotRepository;
        _stadiumRepository = stadiumRepository;
        _emailSender = emailSender;
    }

    public async Task<Result<BookingResponse>> CreateBookingAsync(CreateBookingRequest request, string playerId)
    {
        // 1. التأكد إن الـ TimeSlot موجود ومتاح
        var timeSlot = await _timeSlotRepository.GetByIdAsync(request.TimeSlotId);
        if (timeSlot is null || timeSlot.Status != SlotStatus.Available)
            return Result.Failure<BookingResponse>(BookingErrors.SlotUnavailable);

        // 2. حساب السعر بناءً على سعر الملعب ومدته
        var stadium = await _stadiumRepository.GetByIdAsync(timeSlot.StadiumId);
        if (stadium is null)
            return Result.Failure<BookingResponse>(BookingErrors.StadiumNotFound);

        // حساب المدة بالساعات وضربها في سعر الساعة
        var durationInHours = (timeSlot.EndTime - timeSlot.StartTime).TotalHours;
        var totalPrice = (decimal)durationInHours * stadium.PricePerHour;

        // 3. إنشاء الحجز
        var booking = new Booking
        {
            PlayerId = playerId,
            TimeSlotId = request.TimeSlotId,
            Status = BookingStatus.Pending,
            TotalPrice = totalPrice,
            CreatedOn = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(30) // مهلة 30 دقيقة للدفع
        };
        await _bookingRepository.AddAsync(booking);

        // 4. تغيير حالة الميعاد لمحجوز عشان ميظهرش للمستخدمين تاني
        timeSlot.Status = SlotStatus.Booked;
        await _timeSlotRepository.UpdateAsync(timeSlot);

        var response = booking.Adapt<BookingResponse>();
        return Result.Success(response);
    }

    public async Task<Result> CancelBookingAsync(int bookingId, string cancellationReason, string userId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);

        if (booking is null)
            return Result.Failure(BookingErrors.NotFound);

        if (booking.PlayerId != userId) // حماية: لازم يكون صاحب الحجز
            return Result.Failure(BookingErrors.Unauthorized);

        if (booking.Status == BookingStatus.Cancelled)
            return Result.Failure(BookingErrors.AlreadyCancelled);

        // 1. تغيير حالة الحجز لـ Cancelled
        booking.Status = BookingStatus.Cancelled;
        booking.CancelledAt = DateTime.UtcNow;
        booking.CancellationReason = cancellationReason;
        await _bookingRepository.UpdateAsync(booking);

        // 2. إرجاع الميعاد لـ متاح (Available) عشان يقدر حد تاني يحجزه!
        var timeSlot = booking.TimeSlot ?? await _timeSlotRepository.GetByIdAsync(booking.TimeSlotId);
        if (timeSlot is not null)
        {
            timeSlot.Status = SlotStatus.Available;
            await _timeSlotRepository.UpdateAsync(timeSlot);
        }

        // 3. إرسال إيميل لصاحب الملعب في الخلفية
        if (booking.TimeSlot?.Stadium?.Owner?.Email != null)
        {
            var ownerEmail = booking.TimeSlot.Stadium.Owner.Email;
            var stadiumName = booking.TimeSlot.Stadium.Name;
            var playerName = booking.Player != null ? $"{booking.Player.FirstName} {booking.Player.LastName}" : "لاعب";
            var date = booking.TimeSlot.Date.ToString("yyyy-MM-dd");
            var time = $"{booking.TimeSlot.StartTime} - {booking.TimeSlot.EndTime}";

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(
                ownerEmail,
                "⚠️ إلغاء حجز",
                $"أهلاً بك،<br>قام اللاعب <b>{playerName}</b> بإلغاء حجزه في ملعب <b>{stadiumName}</b>.<br>الموعد المُلغى: {date} من {time}.<br><b>السبب:</b> {cancellationReason}<br>تم إعادة فتح الميعاد ليحجزه لاعب آخر."
            ));
        }

        return Result.Success();
    }

    public async Task<Result<BookingDetailsResponse>> UpdateBookingStatusAsync(int bookingId, UpdateBookingRequest request)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);

        if (booking == null)
            return Result.Failure<BookingDetailsResponse>(BookingErrors.NotFound);

        // لو الحجز مؤكد، ممكن تقيد إنه يتلغي تاني بسهولة (اختياري حسب البيزنس)
        if (booking.Status == BookingStatus.Confirmed && request.Status != BookingStatus.Confirmed)
            return Result.Failure<BookingDetailsResponse>(BookingErrors.AlreadyConfirmed);

        // تحديث الحالة
        booking.Status = request.Status;

        if (request.Status == BookingStatus.Cancelled)
        {
            booking.CancelledAt = DateTime.UtcNow;
            booking.CancellationReason = request.CancellationReason;

            // إرجاع الميعاد لـ متاح
            var timeSlot = booking.TimeSlot ?? await _timeSlotRepository.GetByIdAsync(booking.TimeSlotId);
            if (timeSlot is not null)
            {
                timeSlot.Status = SlotStatus.Available;
                await _timeSlotRepository.UpdateAsync(timeSlot);
            }
        }
        else if (request.Status == BookingStatus.Confirmed)
        {
            booking.ConfirmedAt = DateTime.UtcNow;
        }

        await _bookingRepository.UpdateAsync(booking);

        var response = booking.Adapt<BookingDetailsResponse>();
        return Result.Success(response);
    }

    public async Task<Result> DeleteBookingAsync(int id)
    {
        var exists = await _bookingRepository.ExistsAsync(id);
        if (!exists)
            return Result.Failure(BookingErrors.NotFound);

        await _bookingRepository.DeleteAsync(id);
        return Result.Success();
    }

    public async Task<Result<IEnumerable<BookingDetailsResponse>>> GetAllBookingsAsync()
    {
        var bookings = await _bookingRepository.GetAllAsync();
        return Result.Success(bookings.Adapt<IEnumerable<BookingDetailsResponse>>());
    }

    public async Task<Result<BookingDetailsResponse>> GetBookingByIdAsync(int id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);
        if (booking is null)
            return Result.Failure<BookingDetailsResponse>(BookingErrors.NotFound);

        return Result.Success(booking.Adapt<BookingDetailsResponse>());
    }

    public async Task<Result<IEnumerable<BookingDetailsResponse>>> GetBookingsByDateRangeAsync(DateTime start, DateTime end)
    {
        var bookings = await _bookingRepository.GetByDateRangeAsync(start, end);
        return Result.Success(bookings.Adapt<IEnumerable<BookingDetailsResponse>>());
    }

    public async Task<Result<IEnumerable<BookingDetailsResponse>>> GetBookingsByStadiumAsync(int stadiumId)
    {
        var bookings = await _bookingRepository.GetByStadiumIdAsync(stadiumId);
        return Result.Success(bookings.Adapt<IEnumerable<BookingDetailsResponse>>());
    }

    public async Task<Result<IEnumerable<BookingDetailsResponse>>> GetBookingsByStatusAsync(BookingStatus status)
    {
        var bookings = await _bookingRepository.GetByStatusAsync(status);
        return Result.Success(bookings.Adapt<IEnumerable<BookingDetailsResponse>>());
    }

    public async Task<Result<IEnumerable<BookingDetailsResponse>>> GetBookingsByUserAsync(string userId)
    {
        var bookings = await _bookingRepository.GetByUserIdAsync(userId);
        return Result.Success(bookings.Adapt<IEnumerable<BookingDetailsResponse>>());
    }

    public async Task<Result<bool>> IsTimeSlotBookedAsync(int stadiumId, DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        var isBooked = await _bookingRepository.IsTimeSlotBookedAsync(stadiumId, date, startTime, endTime);
        return Result.Success(isBooked);
    }
}
