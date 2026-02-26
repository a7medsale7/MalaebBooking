using MalaebBooking.Application.Contracts.SportTypes;
using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using Mapster;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Services;

public class SportTypeService : ISportTypeService
{
    private readonly ISportTypeRepository _repo;

    public SportTypeService(ISportTypeRepository repo)
    {
        _repo = repo;
    }

    public async Task<SportTypeResponse> AddAsync(SportTypeRequest request, CancellationToken cancellationToken = default)
    {
        var entity = request.Adapt<SportType>();
        await _repo.AddAsync(entity, cancellationToken);
        return entity.Adapt<SportTypeResponse>();
    }

    public async Task<IEnumerable<SportTypeResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repo.GetAllAsync(cancellationToken);
        return entities.Adapt<List<SportTypeResponse>>();
    }

    public async Task<SportTypeResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repo.GetByIdAsync(id, cancellationToken);
        return entity?.Adapt<SportTypeResponse>();
    }

    public async Task<SportTypeResponse> SoftDeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await _repo.SoftDeleteAsync(id, cancellationToken);
        var entity = await _repo.GetByIdAsync(id, cancellationToken);
        return entity!.Adapt<SportTypeResponse>();
    }

    public async Task<SportTypeResponse> UpdateAsync(int id, UpdateSportTypeRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _repo.GetByIdAsync(id, cancellationToken);
        if (entity == null)
            throw new Exception("SportType not found");

        request.Adapt(entity);
        await _repo.UpdateAsync(entity, cancellationToken);

        return entity.Adapt<SportTypeResponse>();
    }
}