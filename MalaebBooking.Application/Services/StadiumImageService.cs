// MalaebBooking.Application/Services/StadiumImageService.cs

using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.Stadiums;
using MalaebBooking.Application.Errors;
using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Hosting;

namespace MalaebBooking.Application.Services;

public class StadiumImageService(
    IStadiumImageRepository stadiumImageRepository,
    IStadiumRepository stadiumRepository,
    IWebHostEnvironment env) : IStadiumImageService
{
    private readonly IStadiumImageRepository _imageRepository = stadiumImageRepository;
    private readonly IStadiumRepository _stadiumRepository = stadiumRepository;
    private readonly IWebHostEnvironment _env = env;

    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5MB

    // ================== UPLOAD IMAGE ==================
    public async Task<Result<StadiumImageResponse>> UploadImageAsync(
        int stadiumId,
        UploadStadiumImageRequest request,
        string currentUserId,
        CancellationToken cancellationToken = default)
    {
        // التحقق من وجود الملعب وصلاحية المالك
        var stadium = await _stadiumRepository.GetByIdAsync(stadiumId, cancellationToken);
        if (stadium is null)
            return Result.Failure<StadiumImageResponse>(StadiumImageErrors.StadiumNotFound);

        if (stadium.OwnerId != currentUserId)
            return Result.Failure<StadiumImageResponse>(StadiumImageErrors.NotAuthorized);

        // التحقق من الملف
        var file = request.File;
        if (file is null || file.Length == 0)
            return Result.Failure<StadiumImageResponse>(StadiumImageErrors.EmptyFile);

        if (file.Length > MaxFileSizeBytes)
            return Result.Failure<StadiumImageResponse>(StadiumImageErrors.FileTooLarge);

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            return Result.Failure<StadiumImageResponse>(StadiumImageErrors.InvalidFile);

        // حفظ الملف على الـ Disk
        var fileName = $"{Guid.NewGuid()}{extension}";
        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "stadiums");
        Directory.CreateDirectory(uploadsFolder); // هيتجاهل لو الفولدر موجود
        var filePath = Path.Combine(uploadsFolder, fileName);

        try
        {
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);
        }
        catch
        {
            return Result.Failure<StadiumImageResponse>(StadiumImageErrors.UploadFailed);
        }

        // حفظ في قاعدة البيانات
        var image = new StadiumImage
        {
            StadiumId = stadiumId,
            ImageUrl = $"/uploads/stadiums/{fileName}",
            IsPrimary = request.IsPrimary,
            DisplayOrder = request.DisplayOrder
        };

        await _imageRepository.AddAsync(image, cancellationToken);

        return Result.Success(image.Adapt<StadiumImageResponse>());
    }

    // ================== GET ALL IMAGES FOR A STADIUM ==================
    public async Task<Result<List<StadiumImageResponse>>> GetImagesByStadiumAsync(
        int stadiumId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _stadiumRepository.ExistsAsync(stadiumId, cancellationToken);
        if (!exists)
            return Result.Failure<List<StadiumImageResponse>>(StadiumImageErrors.StadiumNotFound);

        var images = await _imageRepository.GetByStadiumIdAsync(stadiumId, cancellationToken);
        return Result.Success(images.Adapt<List<StadiumImageResponse>>());
    }

    // ================== GET SINGLE IMAGE BY ID ==================
    public async Task<Result<StadiumImageResponse>> GetImageByIdAsync(
        int imageId,
        CancellationToken cancellationToken = default)
    {
        var image = await _imageRepository.GetByIdAsync(imageId, cancellationToken);
        if (image is null)
            return Result.Failure<StadiumImageResponse>(StadiumImageErrors.NotFound);

        return Result.Success(image.Adapt<StadiumImageResponse>());
    }

    // ================== UPDATE IMAGE META ==================
    public async Task<Result> UpdateImageAsync(
        int imageId,
        UpdateStadiumImageRequest request,
        string currentUserId,
        CancellationToken cancellationToken = default)
    {
        var image = await _imageRepository.GetByIdAsync(imageId, cancellationToken);
        if (image is null)
            return Result.Failure(StadiumImageErrors.NotFound);

        var stadium = await _stadiumRepository.GetByIdAsync(image.StadiumId, cancellationToken);
        if (stadium is null || stadium.OwnerId != currentUserId)
            return Result.Failure(StadiumImageErrors.NotAuthorized);

        image.IsPrimary = request.IsPrimary;
        image.DisplayOrder = request.DisplayOrder;

        await _imageRepository.UpdateAsync(image, cancellationToken);
        return Result.Success();
    }

    // ================== DELETE IMAGE ==================
    public async Task<Result> DeleteImageAsync(
        int imageId,
        string currentUserId,
        CancellationToken cancellationToken = default)
    {
        var image = await _imageRepository.GetByIdAsync(imageId, cancellationToken);
        if (image is null)
            return Result.Failure(StadiumImageErrors.NotFound);

        var stadium = await _stadiumRepository.GetByIdAsync(image.StadiumId, cancellationToken);
        if (stadium is null || stadium.OwnerId != currentUserId)
            return Result.Failure(StadiumImageErrors.NotAuthorized);

        // حذف الملف من الـ Disk
        var physicalPath = Path.Combine(
            _env.WebRootPath,
            image.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

        if (File.Exists(physicalPath))
            File.Delete(physicalPath);

        // حذف من قاعدة البيانات
        await _imageRepository.DeleteAsync(image, cancellationToken);
        return Result.Success();
    }
}
