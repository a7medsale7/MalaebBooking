using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.Bookings;
using MalaebBooking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Services;

public interface IBookingService
{
    // ==================== Create ====================
    Task<Result<BookingResponse>> CreateBookingAsync(CreateBookingRequest request, string playerId);

    // ==================== Read ====================
    Task<Result<BookingDetailsResponse>> GetBookingByIdAsync(int id);
    Task<Result<IEnumerable<BookingDetailsResponse>>> GetAllBookingsAsync();
    Task<Result<IEnumerable<BookingDetailsResponse>>> GetBookingsByUserAsync(string userId);
    Task<Result<IEnumerable<BookingDetailsResponse>>> GetBookingsByStadiumAsync(int stadiumId);
    Task<Result<IEnumerable<BookingDetailsResponse>>> GetBookingsByStatusAsync(BookingStatus status);
    Task<Result<IEnumerable<BookingDetailsResponse>>> GetBookingsByDateRangeAsync(DateTime start, DateTime end);

    // ==================== Update ====================
    Task<Result<BookingDetailsResponse>> UpdateBookingStatusAsync(int bookingId, UpdateBookingRequest request);

    // ==================== Cancel ====================
    // إضافة ميثود مخصوصة ومسؤولة عن إلغاء الحجز 
    Task<Result> CancelBookingAsync(int bookingId, string cancellationReason, string userId);

    // ==================== Delete ====================
    Task<Result> DeleteBookingAsync(int id);

    // ==================== Utilities ====================
    Task<Result<bool>> IsTimeSlotBookedAsync(int stadiumId, DateOnly date, TimeOnly startTime, TimeOnly endTime);
}
