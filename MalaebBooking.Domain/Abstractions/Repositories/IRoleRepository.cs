using MalaebBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Domain.Abstractions.Repositories;
public interface IRoleRepository
{

    Task<List<ApplicationRole>> GetAllAsync(bool? IncludeDisabled);

    Task<ApplicationRole> GetByIdAsync(string id);
    Task<ApplicationRole> AddAsync(ApplicationRole role, IEnumerable<string> permissions);
    Task<ApplicationRole?> UpdateAsync(string id, string newName, IEnumerable<string> newPermissions);
    Task<ApplicationRole?> ToggleStatusAsync(string id);

}
