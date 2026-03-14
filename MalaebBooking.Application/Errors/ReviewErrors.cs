using MalaebBooking.Application.Abstractions.Result;

namespace MalaebBooking.Application.Errors;

public static class ReviewErrors
{
    public static readonly Error NotFound =
        new("Review.NotFound", "Review not found.");

    public static readonly Error NoReviewsForPlayer =
        new("Review.NoReviewsForPlayer", "No reviews found for this player.");

    public static readonly Error NoReviewsForStadium =
        new("Review.NoReviewsForStadium", "No reviews found for this stadium.");

    public static readonly Error NotAuthorized =
        new("Review.NotAuthorized", "You are not allowed to update or delete this review.");

    public static readonly Error AlreadyReviewed =
        new("Review.AlreadyReviewed", "You have already reviewed this stadium.");

    public static readonly Error NoPriorBooking =
        new("Review.NoPriorBooking", "You can only review a stadium after booking and playing there.");
}
