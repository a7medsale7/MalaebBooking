// MalaebBooking.Application/Contracts/Stadiums/UploadStadiumImageRequest.cs

using Microsoft.AspNetCore.Http;

namespace MalaebBooking.Application.Contracts.Stadiums;

public class UploadStadiumImageRequest
{
    public IFormFile File { get; set; } = default!;
    public bool IsPrimary { get; set; } = false;
    public int DisplayOrder { get; set; } = 0;
}
