using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Services;
public interface IRoleService 
{
    Task<Result<List<RoleResponse>>> GetAllAsync(bool includeDisabled = false);
    Task<Result<RoleDetailResponse>> GetByIdAsync(string id);
    Task<Result<RoleDetailResponse>> AddAsync(RoleReqest request);
    Task<Result> UpdateAsync(string id, RoleReqest request);

    Task<Result> ToggleStatusAsync(string id);

}
