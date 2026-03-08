public class CreateStadiumRequest
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

    // Foreign Keys
    public int SportTypeId { get; set; }
    public string OwnerId { get; set; } = string.Empty; // جديد
}