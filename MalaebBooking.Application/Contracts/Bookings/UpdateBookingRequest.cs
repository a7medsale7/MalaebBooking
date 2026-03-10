using System;
using MalaebBooking.Domain.Enums;
using System.Text.Json.Serialization;

namespace MalaebBooking.Application.Contracts.Bookings;

public class UpdateBookingRequest
{
    // الحالة الجديدة اللي عايزين نحدث بيها الحجز
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public BookingStatus Status { get; set; }

    // اختياري: لو الحالة بقت Cancelled، يفضل نبعت السبب للمستخدم
    public string? CancellationReason { get; set; }
}
