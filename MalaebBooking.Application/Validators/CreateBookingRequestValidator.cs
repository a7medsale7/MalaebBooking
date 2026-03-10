using FluentValidation;
using MalaebBooking.Application.Contracts.Bookings;

namespace MalaebBooking.Application.Validators;

public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(x => x.TimeSlotId)
            .GreaterThan(0)
            .WithMessage("يجب اختيار ميعاد صحيح للحجز.");
    }
}
