using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using MalaebBooking.Domain.Enums;
using MalaebBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.Repositories;
public class StadiumOwnerProfileRepository(ApplicationDbContext context) : IStadiumOwnerProfileRepository
{
    private readonly ApplicationDbContext _context = context;
    public async Task AddAsync(StadiumOwnerProfile profile, CancellationToken cancellationToken = default)
    {
        await _context.StadiumOwnerProfiles.AddAsync(profile, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task UpdateAsync(StadiumOwnerProfile profile, CancellationToken cancellationToken = default)
    {
        _context.StadiumOwnerProfiles.Update(profile);
        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task<StadiumOwnerProfile?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.StadiumOwnerProfiles
            .Include(p => p.User)               // جلب بيانات المالك
            .Include(p => p.ApprovedBy)         // جلب بيانات الأدمن الموثق
            .Include(p => p.Stadiums)           // جلب قائمة الملاعب
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
    public async Task<StadiumOwnerProfile?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.StadiumOwnerProfiles
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
    }
    public async Task<List<StadiumOwnerProfile>> GetPendingProfilesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.StadiumOwnerProfiles
            .AsNoTracking()
            .Include(p => p.User)
            .Where(p => p.Status == StadiumOwnerStatus.Pending)
            .ToListAsync(cancellationToken);
    }
    public async Task<List<StadiumOwnerProfile>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.StadiumOwnerProfiles
            .AsNoTracking()
            .Include(p => p.User)
            .ToListAsync(cancellationToken);
    }
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.StadiumOwnerProfiles.AnyAsync(p => p.Id == id, cancellationToken);
    }
}