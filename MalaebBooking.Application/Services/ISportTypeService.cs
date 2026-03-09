using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.SportTypes;

namespace MalaebBooking.Application.Services;

public interface ISportTypeService
{
    Task<Result<IEnumerable<SportTypeResponse>>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<Result<SportTypeResponse>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<Result<SportTypeResponse>> AddAsync(
        SportTypeRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<SportTypeResponse>> UpdateAsync(
        int id,
        UpdateSportTypeRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<SportTypeResponse>> SoftDeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
    Task<Result<SportTypeResponse>> UploadIconAsync(
       int id,
       UploadSportTypeIconRequest request,
       CancellationToken cancellationToken = default);
}