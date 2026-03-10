using System;
using MalaebBooking.Domain.Enums;
using System.Text.Json.Serialization;

namespace MalaebBooking.Application.Contracts.Bookings;

public class BookingDetailsResponse
{
    public int Id { get; set; }
    
    // معلومات الحجز نفسه
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public BookingStatus Status { get; set; }
    public decimal TotalPrice { get; set; }
    
    public DateTime CreatedOn { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancellationReason { get; set; }

    // معلومات اللاعب
    public string PlayerId { get; set; } = string.Empty;
    public string PlayerName { get; set; } = string.Empty; // هتيجي من علاقة الـ User

    // معلومات الوقت والملعب (Flat structure أسهل للـ UI)
    public int TimeSlotId { get; set; }
    public int StadiumId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
