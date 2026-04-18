using FluentValidation;
using MalaebBooking.Application.Contracts.TimeSlots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Validators;
public class PreviewTimeSlotsRequestValidator : AbstractValidator<PreviewTimeSlotsRequest>
{
    public PreviewTimeSlotsRequestValidator()
    {
        RuleFor(x => x.StadiumId)
     .GreaterThan(0)
     .WithMessage("Stadium must be selected.");

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Date is required.");

        RuleFor(x => x.RangeStart)
            .NotEmpty()
            .WithMessage("Start time is required.");

        RuleFor(x => x.RangeEnd)
            .NotEmpty()
            .WithMessage("End time is required.");
    }
}