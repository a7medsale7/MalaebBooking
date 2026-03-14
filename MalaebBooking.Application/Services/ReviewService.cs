using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.Reviews;
using MalaebBooking.Application.Errors;
using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using MalaebBooking.Domain.Enums;
using Mapster;

namespace MalaebBooking.Application.Services;

public class ReviewService(
    IReviewRepository reviewRepository,
    IBookingRepository bookingRepository) : IReviewService
{
    private readonly IReviewRepository _reviewRepository = reviewRepository;
    private readonly IBookingRepository _bookingRepository = bookingRepository;

    // =========================================================================================
    // ================== PRIVATE HELPER: MAP REVIEW TO RESPONSE ==================
    // =========================================================================================
    // عملنا Helper Method عشان الـ Mapping متتكررش في كل ميثود
    private static ReviewResponse MapToResponse(Review r) => new()
    {
        Id = r.Id,
        Rating = r.Rating,
        Comment = r.Comment,
        StadiumId = r.StadiumId,
        PlayerId = r.PlayerId,
        PlayerName = r.Player?.UserName ?? string.Empty, // من الـ Navigation Property
        CreatedOn = r.CreatedOn // ✅ مش CreatedAt
    };

    // =========================================================================================
    // ================== CHECK IF PLAYER CAN REVIEW ==================
    // =========================================================================================
    public async Task<Result<bool>> CanPlayerReviewStadiumAsync(int stadiumId, string playerId)
    {
        var alreadyReviewed = await _reviewRepository.ExistsAsync(playerId, stadiumId);

        if (alreadyReviewed)
            return Result.Success(false);

        var playerBookings = await _bookingRepository.GetByUserIdAsync(playerId);

        var hasFinishedBooking = playerBookings.Any(b =>
            b.TimeSlot.StadiumId == stadiumId &&
            (b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.Completed));

        if (!hasFinishedBooking)
            return Result.Success(false);

        return Result.Success(true);
    }

    // =========================================================================================
    // ================== CREATE REVIEW ==================
    // =========================================================================================
    public async Task<Result> CreateReviewAsync(CreateReviewRequest request, string playerId)
    {
        // 1. هل قيّمه قبل كدا؟
        var alreadyReviewed = await _reviewRepository.ExistsAsync(playerId, request.StadiumId);
        if (alreadyReviewed)
            return Result.Failure(ReviewErrors.AlreadyReviewed);

        // 2. هل عنده حجز منتهي في الملعب ده؟
        var playerBookings = await _bookingRepository.GetByUserIdAsync(playerId);
        var hasFinishedBooking = playerBookings.Any(b =>
            b.TimeSlot.StadiumId == request.StadiumId &&
            (b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.Completed));

        if (!hasFinishedBooking)
            return Result.Failure(ReviewErrors.NoPriorBooking);

        // 3. Save
        var review = request.Adapt<Review>();
        review.PlayerId = playerId;

        await _reviewRepository.AddAsync(review);

        return Result.Success();
    }

    // =========================================================================================
    // ================== UPDATE REVIEW ==================
    // =========================================================================================
    public async Task<Result> UpdateReviewAsync(int reviewId, UpdateReviewRequest request, string playerId)
    {
        var review = await _reviewRepository.GetByIdAsync(reviewId);

        if (review is null)
            return Result.Failure(ReviewErrors.NotFound);

        if (review.PlayerId != playerId)
            return Result.Failure(ReviewErrors.NotAuthorized);

        review.Rating = request.Rating;
        review.Comment = request.Comment;

        await _reviewRepository.UpdateAsync(review);

        return Result.Success();
    }

    // =========================================================================================
    // ================== DELETE REVIEW ==================
    // =========================================================================================
    public async Task<Result> DeleteReviewAsync(int reviewId, string playerId)
    {
        var review = await _reviewRepository.GetByIdAsync(reviewId);

        if (review is null)
            return Result.Failure(ReviewErrors.NotFound);

        if (review.PlayerId != playerId)
            return Result.Failure(ReviewErrors.NotAuthorized);

        await _reviewRepository.DeleteAsync(review);

        return Result.Success();
    }

    // =========================================================================================
    // ================== GET REVIEW BY ID ==================
    // =========================================================================================
    public async Task<Result<ReviewResponse>> GetReviewByIdAsync(int reviewId)
    {
        var review = await _reviewRepository.GetByIdAsync(reviewId);

        if (review is null)
            return Result.Failure<ReviewResponse>(ReviewErrors.NotFound);

        return Result.Success(MapToResponse(review)); // ✅ بنستخدم الـ Helper
    }

    // =========================================================================================
    // ================== GET REVIEWS FOR STADIUM ==================
    // =========================================================================================
    public async Task<Result<IEnumerable<ReviewResponse>>> GetStadiumReviewsAsync(int stadiumId)
    {
        var reviews = await _reviewRepository.GetByStadiumIdAsync(stadiumId);

        if (!reviews.Any())
            return Result.Failure<IEnumerable<ReviewResponse>>(ReviewErrors.NoReviewsForStadium);

        var response = reviews.Select(MapToResponse); // ✅ بنستخدم الـ Helper

        return Result.Success(response);
    }

    // =========================================================================================
    // ================== GET REVIEWS FOR PLAYER ==================
    // =========================================================================================
    public async Task<Result<IEnumerable<ReviewResponse>>> GetPlayerReviewsAsync(string playerId)
    {
        var reviews = await _reviewRepository.GetByPlayerIdAsync(playerId);

        if (!reviews.Any())
            return Result.Failure<IEnumerable<ReviewResponse>>(ReviewErrors.NoReviewsForPlayer);

        var response = reviews.Select(MapToResponse); // ✅ بنستخدم الـ Helper

        return Result.Success(response);
    }
}
