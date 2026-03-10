using System;
using System.Text.Json.Serialization;
using MalaebBooking.Domain.Enums;

namespace MalaebBooking.Application.Contracts.TimeSlots;

public record UpdateTimeSlotRequest(
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    [property: JsonConverter(typeof(JsonStringEnumConverter))] SlotStatus Status
);