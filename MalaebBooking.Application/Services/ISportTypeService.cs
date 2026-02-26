using MalaebBooking.Application.Contracts.SportTypes;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Services;

public interface ISportTypeService
{
    Task<IEnumerable<SportTypeResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SportTypeResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<SportTypeResponse> AddAsync(SportTypeRequest request, CancellationToken cancellationToken = default);
    Task<SportTypeResponse> UpdateAsync(int id, UpdateSportTypeRequest request, CancellationToken cancellationToken = default);
    Task<SportTypeResponse> SoftDeleteAsync(int id, CancellationToken cancellationToken = default);
}