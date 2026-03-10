using FluentValidation;
using MalaebBooking.Application.Contracts.TimeSlots;
using System;

namespace MalaebBooking.Application.Validators;

public class CreateTimeSlotRequestValidator : AbstractValidator<CreateTimeSlotRequest>
{
    public CreateTimeSlotRequestValidator()
    {
        RuleFor(x => x.StadiumId)
            .GreaterThan(0).WithMessage("يجب إدخال رقم ملعب صحيح.");

        RuleFor(x => x.Date)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("لا يمكن حجز ميعاد في الماضي.");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("يجب إدخال وقت البداية.");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("يجب إدخال وقت النهاية.");

        RuleFor(x => x)
            .Must(x => x.StartTime < x.EndTime)
            .WithMessage("وقت البداية يجب أن يكون قبل وقت النهاية.")
            .When(x => x.StartTime != default && x.EndTime != default);
    }
}
