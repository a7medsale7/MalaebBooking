using FluentValidation;
using MalaebBooking.Application.Contracts.TimeSlots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Validators;
public class BulkCreateTimeSlotsRequestValidator : AbstractValidator<BulkCreateTimeSlotsRequest>
{
    public BulkCreateTimeSlotsRequestValidator()
    {
        RuleFor(x => x.StadiumId)
    .GreaterThan(0)
    .WithMessage("Stadium must be selected.");

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Date is required.");

        RuleFor(x => x.SelectedSlots)
            .NotEmpty()
            .WithMessage("At least one time slot must be selected.");
    }
}