using MalaebBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Abstractions.Repositories;
public interface IStadiumOwnerProfileRepository
{
    Task AddAsync(StadiumOwnerProfile profile, CancellationToken cancellationToken = default);
    Task UpdateAsync(StadiumOwnerProfile profile, CancellationToken cancellationToken = default);
    Task<StadiumOwnerProfile?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<StadiumOwnerProfile?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<List<StadiumOwnerProfile>> GetPendingProfilesAsync(CancellationToken cancellationToken = default);
}
