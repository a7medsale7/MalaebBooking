using FluentValidation;
using MalaebBooking.Application.Contracts.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Validators;
public class SubmitPaymentRequestValidator : AbstractValidator<SubmitPaymentRequest>
{
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
    private const long MaxFileSizeBytes = 5 * 1024 * 1024;

    public SubmitPaymentRequestValidator()
    {
        RuleFor(x => x.Screenshot)
            .Cascade(CascadeMode.Stop)          // ✅ وقف عند أول فشل
            .NotNull().WithMessage("Screenshot is required.")
            .Must(f => f!.Length > 0).WithMessage("Screenshot file cannot be empty.")
            .Must(f => f!.Length <= MaxFileSizeBytes).WithMessage("File size cannot exceed 5MB.")
            .Must(f =>
            {
                var ext = Path.GetExtension(f!.FileName).ToLowerInvariant();
                return AllowedExtensions.Contains(ext);
            }).WithMessage("Only JPG, PNG, WEBP files are allowed.");

        RuleFor(x => x.PlayerPhoneNumber)
     .NotEmpty().WithMessage("Player phone number is required.")
     .MinimumLength(10).WithMessage("Phone number must be at least 10 digits.");


    }
}
