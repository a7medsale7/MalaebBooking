using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using MalaebBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MalaebBooking.Infrastructure.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly ApplicationDbContext _context;

    public ReviewRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Review review)
    {
        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Review review)
    {
        _context.Reviews.Update(review);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Review review)
    {
        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
    }

    // ✅ Include Player عشان نعرف اسم اللاعب في الـ Response
    public async Task<Review?> GetByIdAsync(int id)
    {
        return await _context.Reviews
            .Include(r => r.Player)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    // ✅ Include Player
    public async Task<IEnumerable<Review>> GetByPlayerIdAsync(string playerId)
    {
        return await _context.Reviews
            .Include(r => r.Player)
            .Where(r => r.PlayerId == playerId)
            .ToListAsync();
    }

    // ✅ Include Player
    public async Task<IEnumerable<Review>> GetByStadiumIdAsync(int stadiumId)
    {
        return await _context.Reviews
            .Include(r => r.Player)
            .Where(r => r.StadiumId == stadiumId)
            .ToListAsync();
    }

    // دي مش محتاجة Include لأنها بترجع Review للتحقق بس
    public async Task<Review?> GetUserReviewForStadiumAsync(string playerId, int stadiumId)
    {
        return await _context.Reviews
            .FirstOrDefaultAsync(r => r.PlayerId == playerId && r.StadiumId == stadiumId);
    }

    public async Task<bool> ExistsAsync(string playerId, int stadiumId)
    {
        return await _context.Reviews
            .AnyAsync(r => r.PlayerId == playerId && r.StadiumId == stadiumId);
    }

    public async Task<double> GetAverageRatingForStadiumAsync(int stadiumId)
    {
        return await _context.Reviews
            .Where(r => r.StadiumId == stadiumId)
            .AverageAsync(r => (double?)r.Rating) ?? 0.0;
    }

    public async Task<int> GetReviewCountForStadiumAsync(int stadiumId)
    {
        return await _context.Reviews
            .CountAsync(r => r.StadiumId == stadiumId);
    }
}
