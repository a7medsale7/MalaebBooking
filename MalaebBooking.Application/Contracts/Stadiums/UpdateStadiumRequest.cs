using System;
using System.Collections.Generic;

namespace MalaebBooking.Application.Contracts.Stadiums;
public class UpdateStadiumRequest
{
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

    public List<string>? ImageUrls { get; set; }

    // Foreign Key
    public int SportTypeId { get; set; }
}