using MalaebBooking.Domain.Entities;
using MalaebBooking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Abstractions.Repositories;

public interface IBookingRepository
{
    // ==================== Create ====================
    Task AddAsync(Booking booking);

    // ==================== Read ====================
    Task<Booking?> GetByIdAsync(int id);
    Task<IEnumerable<Booking>> GetAllAsync();

    // Optional: get bookings by user
    Task<IEnumerable<Booking>> GetByUserIdAsync(string userId);

    // Optional: get bookings by stadium
    Task<IEnumerable<Booking>> GetByStadiumIdAsync(int stadiumId);

    // Optional: get bookings by status
    Task<IEnumerable<Booking>> GetByStatusAsync(BookingStatus status);

    // Optional: get bookings by date range
    Task<IEnumerable<Booking>> GetByDateRangeAsync(DateTime start, DateTime end);

    // ===== الميثود المضافة حديثاً =====
    // لجلب الحجز/الحجوزات المرتبطة بميعاد محدد
    Task<IEnumerable<Booking>> GetByTimeSlotIdAsync(int timeSlotId);

    // ==================== Update ====================
    Task UpdateAsync(Booking booking);

    // ==================== Delete ====================
    Task DeleteAsync(int id);

    // ==================== Utilities ====================
    Task<bool> ExistsAsync(int id);

    // Optional: check if a time slot is already booked
    Task<bool> IsTimeSlotBookedAsync(int stadiumId, DateOnly date, TimeOnly startTime, TimeOnly endTime);




    // ===== الميثودات التحليلية =====
    Task<IEnumerable<Booking>> GetConfirmedBookingsInPastAsync();
    Task<IEnumerable<Booking>> GetExpiredPendingBookingsAsync();
    // بتجيب الحجوزات اللي ميعادها كمان x من الوقت
    Task<IEnumerable<Booking>> GetUpcomingBookingsAsync(TimeSpan timeAhead);
}
