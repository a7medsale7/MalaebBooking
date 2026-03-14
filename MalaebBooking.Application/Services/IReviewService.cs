using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.Reviews;

namespace MalaebBooking.Application.Services;

public interface IReviewService
{
    // ================== CREATE REVIEW ==================
    Task<Result> CreateReviewAsync(
        CreateReviewRequest request,
        string playerId);

    // ================== UPDATE REVIEW ==================
    Task<Result> UpdateReviewAsync(
        int reviewId,
        UpdateReviewRequest request,
        string playerId);

    // ================== DELETE REVIEW ==================
    Task<Result> DeleteReviewAsync(
        int reviewId,
        string playerId);

    // ================== GET REVIEW BY ID ==================
    Task<Result<ReviewResponse>> GetReviewByIdAsync(int reviewId);

    // ================== GET REVIEWS FOR STADIUM ==================
    Task<Result<IEnumerable<ReviewResponse>>> GetStadiumReviewsAsync(int stadiumId);

    // ================== GET REVIEWS FOR PLAYER ==================
    Task<Result<IEnumerable<ReviewResponse>>> GetPlayerReviewsAsync(string playerId);

    // ================== CHECK IF PLAYER CAN REVIEW ==================
    Task<Result<bool>> CanPlayerReviewStadiumAsync(int stadiumId, string playerId);
}