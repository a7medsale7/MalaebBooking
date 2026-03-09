// MalaebBooking.Application/Services/IStadiumImageService.cs

using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.Stadiums;

namespace MalaebBooking.Application.Services;

public interface IStadiumImageService
{
    // ================== UPLOAD IMAGE ==================
    Task<Result<StadiumImageResponse>> UploadImageAsync(
        int stadiumId,
        UploadStadiumImageRequest request,
        string currentUserId,
        CancellationToken cancellationToken = default);

    // ================== GET ALL IMAGES FOR A STADIUM ==================
    Task<Result<List<StadiumImageResponse>>> GetImagesByStadiumAsync(
        int stadiumId,
        CancellationToken cancellationToken = default);

    // ================== GET SINGLE IMAGE BY ID ==================
    Task<Result<StadiumImageResponse>> GetImageByIdAsync(
        int imageId,
        CancellationToken cancellationToken = default);

    // ================== UPDATE IMAGE META (IsPrimary / DisplayOrder) ==================
    Task<Result> UpdateImageAsync(
        int imageId,
        UpdateStadiumImageRequest request,
        string currentUserId,
        CancellationToken cancellationToken = default);

    // ================== DELETE IMAGE ==================
    Task<Result> DeleteImageAsync(
        int imageId,
        string currentUserId,
        CancellationToken cancellationToken = default);
}
