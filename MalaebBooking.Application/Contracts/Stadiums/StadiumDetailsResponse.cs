using MalaebBooking.Application.Contracts.Reviews;
using MalaebBooking.Application.Contracts.TimeSlots;
using System;
using System.Collections.Generic;

namespace MalaebBooking.Application.Contracts.Stadiums;
public class StadiumDetailsResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Address { get; set; } = string.Empty;
    public string? GoogleMapsUrl { get; set; }
    public decimal PricePerHour { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string? InstapayNumber { get; set; }

    public TimeOnly OpeningTime { get; set; }
    public TimeOnly ClosingTime { get; set; }
    public int SlotDurationMinutes { get; set; } = 60;
    public bool IsActive { get; set; }

    // Foreign Key info
    public int SportTypeId { get; set; }
    public string SportTypeName { get; set; } = string.Empty;

    // Navigation
    public string OwnerId { get; set; } = string.Empty;
    public string? OwnerName { get; set; }

    public List<string>? ImageUrls { get; set; }

    // Related Collections
    public List<TimeSlotResponse>? TimeSlots { get; set; }
    public List<ReviewResponse>? Reviews { get; set; }
}