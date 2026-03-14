using MalaebBooking.Domain.Entities;

namespace MalaebBooking.Domain.Abstractions.Repositories;

public interface IReviewRepository
{
    // ================== CREATE ==================
    Task AddAsync(Review review);

    // ================== UPDATE ==================
    Task UpdateAsync(Review review);

    // ================== DELETE ==================
    Task DeleteAsync(Review review);

    // ================== GET BY ID ==================
    Task<Review?> GetByIdAsync(int id);

    // ================== GET ALL REVIEWS FOR STADIUM ==================
    Task<IEnumerable<Review>> GetByStadiumIdAsync(int stadiumId);

    // ================== GET USER REVIEW FOR STADIUM ==================
    Task<Review?> GetUserReviewForStadiumAsync(string playerId, int stadiumId);

    // ================== CHECK IF USER REVIEWED STADIUM ==================
    Task<bool> ExistsAsync(string playerId, int stadiumId);

    // ================== GET ALL REVIEWS BY USER ==================
    Task<IEnumerable<Review>> GetByPlayerIdAsync(string playerId);

    // ================== GET STADIUM AVERAGE RATING ==================
    Task<double> GetAverageRatingForStadiumAsync(int stadiumId);

    // ================== COUNT REVIEWS FOR STADIUM ==================
    Task<int> GetReviewCountForStadiumAsync(int stadiumId);
}
