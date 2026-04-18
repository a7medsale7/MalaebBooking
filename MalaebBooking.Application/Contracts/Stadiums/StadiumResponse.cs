using MalaebBooking.Application.Contracts.Stadiums;

public class StadiumResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Address { get; set; } = string.Empty;
    public string? GoogleMapsUrl { get; set; }
    public string Governorate { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? InstapayNumber { get; set; }
    public string? VodafoneCashNumber { get; set; }
    public decimal PricePerHourDay { get; set; }
    public decimal PricePerHourNight { get; set; }
    public TimeOnly SummerNightStartTime { get; set; }
    public TimeOnly WinterNightStartTime { get; set; }
    public string? Dimensions { get; set; }
    public string? CourtType { get; set; }
    public TimeOnly OpeningTime { get; set; }
    public TimeOnly ClosingTime { get; set; }
    public int SlotDurationMinutes { get; set; } = 60;
    public bool IsActive { get; set; }
    public int SportTypeId { get; set; }
    public string SportTypeName { get; set; } = string.Empty;
    public string OwnerId { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public List<StadiumImageResponse>? Images { get; set; }
}