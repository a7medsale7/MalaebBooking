using FluentValidation;
using MalaebBooking.Application.Contracts.TimeSlots;
using System;

namespace MalaebBooking.Application.Validators;

public class UpdateTimeSlotRequestValidator : AbstractValidator<UpdateTimeSlotRequest>
{
    public UpdateTimeSlotRequestValidator()
    {
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
            
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("حالة الميعاد غير صحيحة.");
    }
}
