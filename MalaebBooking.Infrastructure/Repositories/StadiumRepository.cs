using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using MalaebBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.Repositories;

public class StadiumRepository(ApplicationDbContext context) : IStadiumRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddAsync(Stadium stadium, CancellationToken cancellationToken = default)
    {
        await _context.Stadiums.AddAsync(stadium, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Stadiums
            .AnyAsync(st => st.Id == id, cancellationToken);
    }

    public async Task<List<Stadium>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Stadiums
            .AsNoTracking()
            .Include(s => s.Images)
            .Include(s => s.SportType)         // 👈 Addition
            .Include(s => s.OwnerProfile)      // جلب البروفايل
                .ThenInclude(p => p.User)      // جلب بيانات اليوزر (الاسم والموبايل)
            .Where(x => x.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Stadium>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Stadiums
            .AsNoTracking()
            .Include(s => s.Images)
            .Include(s => s.SportType)
            .Include(s => s.OwnerProfile)
                .ThenInclude(p => p.User)
            .ToListAsync(cancellationToken);
    }

    public async Task<Stadium?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Stadiums
            .Include(s => s.Images)
            .Include(s => s.SportType)
            .Include(s => s.OwnerProfile)
                .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<Stadium>> GetBySportTypeAsync(int sportTypeId, CancellationToken cancellationToken = default)
    {
        return await _context.Stadiums
            .AsNoTracking()
            .Include(s => s.Images)
            .Include(s => s.SportType)
            .Include(s => s.OwnerProfile)
                .ThenInclude(p => p.User)
            .Where(x => x.SportTypeId == sportTypeId && x.IsActive)
            .ToListAsync(cancellationToken);
    }

    public Task<Stadium?> GetDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Stadiums
            .AsNoTracking()
            .Include(s => s.SportType)
            .Include(s => s.OwnerProfile)       // تصحيح: اسم الخاصية وليس الـ Id
                .ThenInclude(p => p.User)
            .Include(s => s.Images)
            .Include(s => s.TimeSlots)
            .Include(s => s.Reviews)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Stadium stadium, CancellationToken cancellationToken = default)
    {
        _context.Stadiums.Update(stadium);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
