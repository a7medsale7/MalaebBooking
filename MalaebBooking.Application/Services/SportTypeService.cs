using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.SportTypes;
using MalaebBooking.Application.Errors;
using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using Mapster;
using Microsoft.Extensions.Caching.Hybrid;

namespace MalaebBooking.Application.Services;

public class SportTypeService : ISportTypeService
{
    private readonly ISportTypeRepository _repo;
    private readonly HybridCache _hybridCache;

    public SportTypeService(ISportTypeRepository repo , HybridCache hybridCache)
    {
        _repo = repo;
        _hybridCache = hybridCache;
    }

    public async Task<Result<SportTypeResponse>> AddAsync(
         SportTypeRequest request,
         CancellationToken cancellationToken = default)
    {
        // 1️⃣ تحقق إذا الاسم موجود بالفعل
        var exists = await _repo.GetAllAsync(cancellationToken);
        if (exists.Any(x => x.Name.ToLower() == request.Name.ToLower() && x.IsActive))
            return Result.Failure<SportTypeResponse>(SportTypeErrors.AlreadyExists);

        // 2️⃣ تحويل Request ل Entity
        var entity = request.Adapt<SportType>();

        // 3️⃣ إضافة الكيان
        await _repo.AddAsync(entity, cancellationToken);

        // 4️⃣ تحويل Entity ل Response
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

        var response = entity.Adapt<SportTypeResponse>();

        return Result.Success(response);
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
        // التحقق من الـ id
        if (id <= 0)
            return Result.Failure<SportTypeResponse>(SportTypeErrors.InvalidId);

        // جلب الكيان
        var entity = await _repo.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return Result.Failure<SportTypeResponse>(SportTypeErrors.NotFound);

        // تحقق إذا الاسم الجديد متكرر مع أي كيان آخر ما عدا الكيان الحالي
        var allEntities = await _repo.GetAllAsync(cancellationToken);
        if (allEntities.Any(x => x.Id != id && x.IsActive && x.Name.ToLower() == request.Name.ToLower()))
            return Result.Failure<SportTypeResponse>(SportTypeErrors.AlreadyExists);

        // تطبيق التعديلات
        request.Adapt(entity);

        await _repo.UpdateAsync(entity, cancellationToken);

        var response = entity.Adapt<SportTypeResponse>();
        return Result.Success(response);
    }
}