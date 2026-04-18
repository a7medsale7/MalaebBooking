using FluentValidation;
using MalaebBooking.Application.Contracts.StadiumOwner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Validators;
public class ApplyForStadiumOwnerValidator : AbstractValidator<ApplyForStadiumOwnerRequest>
{
    public ApplyForStadiumOwnerValidator()
    {
        RuleFor(x => x.NationalId)
            .NotEmpty().WithMessage("National ID is required")
            .Length(14).WithMessage("National ID must be exactly 14 digits");
        RuleFor(x => x.NationalIdImageFront)
            .NotEmpty().WithMessage("Front side of National ID is required");
        RuleFor(x => x.NationalIdImageBack)
            .NotEmpty().WithMessage("Back side of National ID is required");
    }
}