using System;

namespace MalaebBooking.Application.Contracts.TimeSlots;

public record CreateTimeSlotRequest(
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int StadiumId
);
