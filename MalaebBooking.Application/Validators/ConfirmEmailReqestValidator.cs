using FluentValidation;
using MalaebBooking.Application.Contracts.Auth;

namespace MalaebBooking.Application.Validators;

public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailReqest>
{
    public ConfirmEmailRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Confirmation code is required.")
            .MinimumLength(10)
            .WithMessage("Invalid confirmation code.");
    }
}