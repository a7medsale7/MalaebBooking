using FluentValidation;
using MalaebBooking.Application.Contracts.Bookings;
using MalaebBooking.Domain.Enums;

namespace MalaebBooking.Application.Validators;

public class UpdateBookingRequestValidator : AbstractValidator<UpdateBookingRequest>
{
    public UpdateBookingRequestValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("حالة الحجز المدخلة غير صحيحة.");

        RuleFor(x => x.CancellationReason)
            .MaximumLength(500)
            .WithMessage("سبب الإلغاء يجب ألا يتجاوز 500 حرف.")
            // القاعدة دي بتشتغل بس لو اليوزر كاتب سبب الإلغاء فعلاً
            .When(x => !string.IsNullOrWhiteSpace(x.CancellationReason));

        // Rule إضافية لو حابب تلزمه يكتب سبب لو بيلغي:
        // RuleFor(x => x.CancellationReason)
        //     .NotEmpty()
        //     .WithMessage("يجب توضيح سبب الإلغاء.")
        //     .When(x => x.Status == BookingStatus.Cancelled);
    }
}
