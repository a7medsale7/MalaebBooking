using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.SportTypes;
using MalaebBooking.Application.Errors;
using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using Mapster;

namespace MalaebBooking.Application.Services;

public class SportTypeService : ISportTypeService
{
    private readonly ISportTypeRepository _repo;

    public SportTypeService(ISportTypeRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<SportTypeResponse>> AddAsync(
        SportTypeRequest request,
        CancellationToken cancellationToken = default)
    {
        var entity = request.Adapt<SportType>();
        if(request.Name == entity.Name)
            return Result.Failure<SportTypeResponse>(SportTypeErrors.AlreadyExists);

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
        if (id <= 0)
            return Result.Failure<SportTypeResponse>(SportTypeErrors.InvalidId);

        var entity = await _repo.GetByIdAsync(id, cancellationToken);

        if (entity is null)
            return Result.Failure<SportTypeResponse>(SportTypeErrors.NotFound);

        request.Adapt(entity);

        await _repo.UpdateAsync(entity, cancellationToken);

        return Result.Success(entity.Adapt<SportTypeResponse>());
    }
}