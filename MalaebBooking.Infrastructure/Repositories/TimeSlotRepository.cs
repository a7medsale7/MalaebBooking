using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using MalaebBooking.Domain.Enums;
using MalaebBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MalaebBooking.Infrastructure.Repositories;

public class TimeSlotRepository : ITimeSlotRepository
{
    private readonly ApplicationDbContext _context;

    public TimeSlotRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TimeSlot timeSlot)
    {
        await _context.TimeSlots.AddAsync(timeSlot);
        await _context.SaveChangesAsync();
    }

    public Task DeleteAsync(TimeSlot timeSlot)
    {
        _context.TimeSlots.Remove(timeSlot);
        return _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int slotId)
    {
        return await _context.TimeSlots
            .AnyAsync(x => x.Id == slotId);
    }

    public async Task<IEnumerable<TimeSlot>> GetAllByStadiumIdAsync(int stadiumId)
    {
        return await _context.TimeSlots
            .Include(x => x.Stadium)
            .AsNoTracking()
            .Where(x => x.StadiumId == stadiumId)
            .ToListAsync();
    }

    public async Task<IEnumerable<TimeSlot>> GetByStadiumAndDateAsync(int stadiumId, DateOnly date)
    {
        return await _context.TimeSlots
            .Include(x => x.Stadium)
            .AsNoTracking()
            .Where(x => x.StadiumId == stadiumId && x.Date == date)
            .ToListAsync();
    }

    public async Task<IEnumerable<TimeSlot>> GetAvailableByStadiumAndDateAsync(int stadiumId, DateOnly date)
    {
        return await _context.TimeSlots
            .Include(x => x.Stadium)
            .AsNoTracking()
            .Where(x =>
                x.StadiumId == stadiumId &&
                x.Date == date &&
                x.Status == SlotStatus.Available)
            .ToListAsync();
    }

    public async Task<TimeSlot?> GetByIdAsync(int id)
    {
        return await _context.TimeSlots
            .Include(x => x.Stadium)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> HasOverlappingSlotsAsync(
        int stadiumId,
        DateOnly date,
        TimeOnly startTime,
        TimeOnly endTime)
    {
        return await _context.TimeSlots.AnyAsync(x =>
            x.StadiumId == stadiumId &&
            x.Date == date &&
            startTime < x.EndTime &&
            endTime > x.StartTime);
    }

    public async Task<bool> IsSlotAvailableAsync(int slotId)
    {
        return await _context.TimeSlots
            .AnyAsync(x =>
                x.Id == slotId &&
                x.Status == SlotStatus.Available);
    }

    public async Task UpdateAsync(TimeSlot timeSlot)
    {
        _context.TimeSlots.Update(timeSlot);
        await _context.SaveChangesAsync();
    }
}