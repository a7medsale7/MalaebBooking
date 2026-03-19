using FluentValidation;
using MalaebBooking.Application.Contracts.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Validators;
public class RoleRequestValidator : AbstractValidator<RoleReqest>
{
    public RoleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Role name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Permissions)
            .NotNull().WithMessage("Permissions list is required.")
            .NotEmpty().WithMessage("Permissions must not be empty.")
            .Must(p => p.Distinct().Count() == p.Count())
            .WithMessage("Permissions must not contain duplicates.");

        RuleForEach(x => x.Permissions)
            .NotEmpty().WithMessage("Permission cannot be empty.")
            .MaximumLength(100);
    }
}