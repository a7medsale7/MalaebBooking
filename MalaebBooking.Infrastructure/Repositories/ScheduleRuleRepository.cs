using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using MalaebBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.Repositories;
public class ScheduleRuleRepository : IScheduleRuleRepository
{
    private readonly ApplicationDbContext _context;
    public ScheduleRuleRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(ScheduleRule scheduleRule)
    {
        await _context.ScheduleRules.AddAsync(scheduleRule);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(ScheduleRule scheduleRule)
    {
        _context.ScheduleRules.Remove(scheduleRule);
        await _context.SaveChangesAsync();
    }
    public async Task<ScheduleRule?> GetByIdAsync(int id)
    {
        return await _context.ScheduleRules
            .Include(x => x.Stadium)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<ScheduleRule?> GetByStadiumIdAsync(int stadiumId)
    {
        return await _context.ScheduleRules
            .FirstOrDefaultAsync(x => x.StadiumId == stadiumId && x.IsActive);
    }
    public async Task UpdateAsync(ScheduleRule scheduleRule)
    {
        _context.ScheduleRules.Update(scheduleRule);
        await _context.SaveChangesAsync();
    }
}