using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using MalaebBooking.Domain.Enums;
using MalaebBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;

    public BookingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // ==================== Create ====================
    public async Task AddAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
    }

    // ==================== Read ====================
    public async Task<Booking?> GetByIdAsync(int id)
    {
        return await _context.Bookings
            .Include(x => x.TimeSlot)
            .Include(x => x.Player)
            .Include(x => x.TimeSlot) // يفضل جلب تفاصيل التايم سلوت دايماً مع الحجز
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Booking>> GetAllAsync()
    {
        return await _context.Bookings
            .Include(x => x.TimeSlot)
            .Include(x => x.Player)
            .Include(x => x.TimeSlot)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetByUserIdAsync(string userId)
    {
        return await _context.Bookings
           .Include(x => x.TimeSlot)
            .Include(x => x.Player)
            .Include(x => x.TimeSlot)
            .Where(x => x.PlayerId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetByStadiumIdAsync(int stadiumId)
    {
        return await _context.Bookings
            .Include(x => x.TimeSlot)
            .Include(x => x.Player)
            .Include(x => x.TimeSlot)
            .Where(x => x.TimeSlot.StadiumId == stadiumId)
            .AsNoTracking()
            .ToListAsync();
    }

    // تعديل: BookingStatus بدل SlotStatus
    public async Task<IEnumerable<Booking>> GetByStatusAsync(BookingStatus status)
    {
        return await _context.Bookings
            .Include(x => x.TimeSlot)
            .Include(x => x.Player)
            .Include(x => x.TimeSlot)
            .Where(x => x.Status == status)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetByDateRangeAsync(DateTime start, DateTime end)
    {
        return await _context.Bookings
           .Include(x => x.TimeSlot)
            .Include(x => x.Player)
            .Include(x => x.TimeSlot)
            .Where(x => x.CreatedOn >= start && x.CreatedOn <= end) // تعديل: مقارنة صحيحة لدعم الـ Translation في EF 
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetByTimeSlotIdAsync(int timeslotId)
    {
        return await _context.Bookings
            .Where(x => x.TimeSlotId == timeslotId)
            .AsNoTracking()
            .ToListAsync();
    }

    // ==================== Update ====================
    public async Task UpdateAsync(Booking booking)
    {
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
    }

    // ==================== Delete ====================
    public async Task DeleteAsync(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }
    }

    // ==================== Utilities ====================
    public async Task<bool> ExistsAsync(int id)
    {
        // أسرع بكتير من جلب الأوبجيكت من الداتابيز
        return await _context.Bookings.AnyAsync(x => x.Id == id);
    }

    // تعديل: استخدام DateOnly و TimeOnly عشان يكونوا متناسقين مع TimeSlot 
    // وتعديل حجز الاستيتس لـ BookingStatus.Cancelled
    public async Task<bool> IsTimeSlotBookedAsync(int stadiumId, DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        return await _context.Bookings.AnyAsync(b =>
            b.TimeSlot.StadiumId == stadiumId &&
            b.TimeSlot.Date == date &&
            b.Status != BookingStatus.Cancelled &&
            b.TimeSlot.StartTime < endTime &&
            b.TimeSlot.EndTime > startTime
        );
    }

   
}
