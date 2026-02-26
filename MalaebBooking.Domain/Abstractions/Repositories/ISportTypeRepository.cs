using MalaebBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Abstractions.Repositories;
public interface ISportTypeRepository
{
    Task<IEnumerable<SportType>> GetAllAsync(CancellationToken cancellationToken);
    Task<SportType?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(SportType sportType, CancellationToken cancellationToken);
    Task UpdateAsync(SportType sportType, CancellationToken cancellationToken);
    Task SoftDeleteAsync(int id, CancellationToken cancellationToken);
}
