// MalaebBooking.Application/Contracts/Stadiums/UpdateStadiumImageRequest.cs

namespace MalaebBooking.Application.Contracts.Stadiums;

public class UpdateStadiumImageRequest
{
    public bool IsPrimary { get; set; }
    public int DisplayOrder { get; set; }
}
