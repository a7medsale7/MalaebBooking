using FluentValidation;
using MalaebBooking.Application.Contracts.SportTypes;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Validators;
public class AddSportTypeValidator : AbstractValidator<SportTypeRequest>
{
  
    public AddSportTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
