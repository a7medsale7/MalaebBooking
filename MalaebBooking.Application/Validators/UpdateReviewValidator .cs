using FluentValidation;
using MalaebBooking.Application.Contracts.Reviews;

namespace MalaebBooking.Application.Validators;

public class UpdateReviewValidator : AbstractValidator<UpdateReviewRequest>
{
    public UpdateReviewValidator()
    {
        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5.");

        RuleFor(x => x.Comment)
            .MaximumLength(500)
            .WithMessage("Comment cannot exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Comment));
    }
}