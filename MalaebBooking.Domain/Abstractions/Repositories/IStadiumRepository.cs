using MalaebBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Abstractions.Repositories;
public interface IStadiumRepository
{
    Task AddAsync(Stadium stadium, CancellationToken cancellationToken = default);

    Task UpdateAsync(Stadium stadium, CancellationToken cancellationToken = default);

    Task<Stadium?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Stadium?> GetDetailsAsync(int id, CancellationToken cancellationToken = default);

    Task<List<Stadium>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<List<Stadium>> GetActiveAsync(CancellationToken cancellationToken = default);

    Task<List<Stadium>> GetBySportTypeAsync(int sportTypeId, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}