using System;
using MalaebBooking.Domain.Enums;
using System.Text.Json.Serialization;

namespace MalaebBooking.Application.Contracts.Bookings;

public class BookingResponse
{
    public int Id { get; set; }
    public int TimeSlotId { get; set; }
    public string PlayerId { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }

    // يفضل ترجيعها كـ String في الـ JSON بدل رقم
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public BookingStatus Status { get; set; }

    public DateTime CreatedOn { get; set; }
    public DateTime ExpiresAt { get; set; }
}
