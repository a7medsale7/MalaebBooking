// MalaebBooking.Infrastructure/Repositories/StadiumImageRepository.cs

using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using MalaebBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MalaebBooking.Infrastructure.Repositories;

public class StadiumImageRepository(ApplicationDbContext context) : IStadiumImageRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddAsync(StadiumImage image, CancellationToken cancellationToken = default)
    {
        await _context.StadiumImages.AddAsync(image, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<StadiumImage?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.StadiumImages
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<StadiumImage>> GetByStadiumIdAsync(int stadiumId, CancellationToken cancellationToken = default)
    {
        return await _context.StadiumImages
            .AsNoTracking()
            .Where(x => x.StadiumId == stadiumId)
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(StadiumImage image, CancellationToken cancellationToken = default)
    {
        _context.StadiumImages.Update(image);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(StadiumImage image, CancellationToken cancellationToken = default)
    {
        _context.StadiumImages.Remove(image);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.StadiumImages
            .AnyAsync(x => x.Id == id, cancellationToken);
    }
}
