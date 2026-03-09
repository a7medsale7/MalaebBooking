// MalaebBooking.Application/Services/SportTypeService.cs

using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.SportTypes;
using MalaebBooking.Application.Errors;
using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Hybrid;

namespace MalaebBooking.Application.Services;

public class SportTypeService : ISportTypeService
{
    private readonly ISportTypeRepository _repo;
    private readonly HybridCache _hybridCache;
    private readonly IWebHostEnvironment _env;

    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5MB

    public SportTypeService(ISportTypeRepository repo, HybridCache hybridCache, IWebHostEnvironment env)
    {
        _repo = repo;
        _hybridCache = hybridCache;
        _env = env;
    }

    public async Task<Result<SportTypeResponse>> AddAsync(
        SportTypeRequest request,
        CancellationToken cancellationToken = default)
    {
        var exists = await _repo.GetAllAsync(cancellationToken);
        if (exists.Any(x => x.Name.ToLower() == request.Name.ToLower() && x.IsActive))
            return Result.Failure<SportTypeResponse>(SportTypeErrors.AlreadyExists);

        var entity = request.Adapt<SportType>();
        await _repo.AddAsync(entity, cancellationToken);

        var response = entity.Adapt<SportTypeResponse>();
        return Result.Success(response);
    }

    public async Task<Result<IEnumerable<SportTypeResponse>>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _repo.GetAllAsync(cancellationToken);
        var response = entities.Adapt<List<SportTypeResponse>>();
        return Result.Success<IEnumerable<SportTypeResponse>>(response);
    }

    public async Task<Result<SportTypeResponse>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return Result.Failure<SportTypeResponse>(SportTypeErrors.InvalidId);

        var entity = await _repo.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return Result.Failure<SportTypeResponse>(SportTypeErrors.NotFound);

        return Result.Success(entity.Adapt<SportTypeResponse>());
    }

    public async Task<Result<SportTypeResponse>> SoftDeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return Result.Failure<SportTypeResponse>(SportTypeErrors.InvalidId);

        var entity = await _repo.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return Result.Failure<SportTypeResponse>(SportTypeErrors.NotFound);

        await _repo.SoftDeleteAsync(id, cancellationToken);
        return Result.Success(entity.Adapt<SportTypeResponse>());
    }

    public async Task<Result<SportTypeResponse>> UpdateAsync(
        int id,
        UpdateSportTypeRequest request,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return Result.Failure<SportTypeResponse>(SportTypeErrors.InvalidId);

        var entity = await _repo.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return Result.Failure<SportTypeResponse>(SportTypeErrors.NotFound);

        var allEntities = await _repo.GetAllAsync(cancellationToken);
        if (allEntities.Any(x => x.Id != id && x.IsActive && x.Name.ToLower() == request.Name.ToLower()))
            return Result.Failure<SportTypeResponse>(SportTypeErrors.AlreadyExists);

        request.Adapt(entity);
        await _repo.UpdateAsync(entity, cancellationToken);

        return Result.Success(entity.Adapt<SportTypeResponse>());
    }

    // ================== UPLOAD ICON ==================
    public async Task<Result<SportTypeResponse>> UploadIconAsync(
        int id,
        UploadSportTypeIconRequest request,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return Result.Failure<SportTypeResponse>(SportTypeErrors.InvalidId);

        var entity = await _repo.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return Result.Failure<SportTypeResponse>(SportTypeErrors.NotFound);

        var file = request.File;
        if (file is null || file.Length == 0)
            return Result.Failure<SportTypeResponse>(SportTypeErrors.EmptyFile);

        if (file.Length > MaxFileSizeBytes)
            return Result.Failure<SportTypeResponse>(SportTypeErrors.FileTooLarge);

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            return Result.Failure<SportTypeResponse>(SportTypeErrors.InvalidFile);

        // حذف الأيقونة القديمة من الـ Disk لو موجودة
        if (!string.IsNullOrEmpty(entity.IconUrl))
        {
            var oldPath = Path.Combine(
                _env.WebRootPath,
                entity.IconUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(oldPath))
                File.Delete(oldPath);
        }

        // حفظ الملف الجديد
        var fileName = $"{Guid.NewGuid()}{extension}";
        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "sport-types");
        Directory.CreateDirectory(uploadsFolder);
        var filePath = Path.Combine(uploadsFolder, fileName);

        try
        {
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);
        }
        catch
        {
            return Result.Failure<SportTypeResponse>(SportTypeErrors.UploadFailed);
        }

        // تحديث الـ IconUrl في قاعدة البيانات
        entity.IconUrl = $"/uploads/sport-types/{fileName}";
        await _repo.UpdateAsync(entity, cancellationToken);

        return Result.Success(entity.Adapt<SportTypeResponse>());
    }
}
