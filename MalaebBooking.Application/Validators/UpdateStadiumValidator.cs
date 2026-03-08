using FluentValidation;
using MalaebBooking.Application.Contracts.Stadiums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Validators;
public class UpdateStadiumValidator : AbstractValidator<UpdateStadiumRequest>
{
    public UpdateStadiumValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Stadium name is required.")
            .MaximumLength(100).WithMessage("Stadium name must be less than 100 characters.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(200).WithMessage("Address must be less than 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must be less than 1000 characters.");

        RuleFor(x => x.PricePerHour)
            .GreaterThan(0).WithMessage("Price per hour must be greater than 0.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?\d{7,15}$").WithMessage("Invalid phone number format.");

        RuleFor(x => x.OpeningTime)
            .LessThan(x => x.ClosingTime)
            .WithMessage("Opening time must be before closing time.");

        RuleFor(x => x.SlotDurationMinutes)
            .GreaterThan(0).WithMessage("Slot duration must be greater than 0.");

        RuleFor(x => x.SportTypeId)
            .GreaterThan(0).WithMessage("Sport type must be selected.");

        RuleForEach(x => x.ImageUrls)
            .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("Invalid image URL format.");
    }
}
