namespace MalaebBooking.Application.Contracts.Stadiums;

public class StadiumFilters : RequestFilters
{
    public string? Governorate { get; set; }
    public string? District { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
