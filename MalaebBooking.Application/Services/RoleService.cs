using MalaebBooking.Application.Abstractions.Result;
using MalaebBooking.Application.Contracts.Roles;
using MalaebBooking.Application.Errors;
using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Consts;
using MalaebBooking.Domain.Entities;
using MalaebBooking.Infrastructure.Repositories;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Application.Services;
public class RoleService(IRoleRepository roleRepository , RoleManager<ApplicationRole> roleManager) : IRoleService
{
    private readonly IRoleRepository roleRepository = roleRepository;
    private readonly RoleManager<ApplicationRole> roleManager = roleManager;

    public async Task<Result<List<RoleResponse>>> GetAllAsync(bool includeDisabled = false)
    {
        var roles = await roleRepository.GetAllAsync(includeDisabled);
        var response = roles.Adapt<List<RoleResponse>>();
        return Result<List<RoleResponse>>.Success(response);
    }

    public async Task<Result<RoleDetailResponse>> GetByIdAsync(string id)
    {
        var role = await roleRepository.GetByIdAsync(id);

        if (role == null)
            return Result<RoleDetailResponse>.Failure(RoleErrors.NotFound);

        // جلب الـ claims الخاصة بالـ permissions
        var claims = await roleManager.GetClaimsAsync(role);
        var permissions = claims
            .Where(c => string.Equals(c.Type, Permissions.Type, System.StringComparison.OrdinalIgnoreCase))
            .Select(c => c.Value)
            .ToList();

        // بناء الـ Response
        var response = new RoleDetailResponse
        {
            Id = role.Id.ToString(),
            Name = role.Name ?? string.Empty,
            IsDeleted = role.IsDeleted,
            Permissions = permissions
        };
        return Result<RoleDetailResponse>.Success(response);
    }

    public async Task<Result<RoleDetailResponse>> AddAsync(RoleReqest request)
    {
        // check role exists (case-insensitive via RoleManager)
        var roleIsExists = await roleManager.RoleExistsAsync(request.Name);

        if (roleIsExists)
            return Result.Failure<RoleDetailResponse>(RoleErrors.DuplicatedRole);

        // validate permissions
        var allowedPermissions = Permissions.GetAllPermissions();

        if (request.Permissions.Except(allowedPermissions).Any())
            return Result.Failure<RoleDetailResponse>(RoleErrors.InvalidPermissions);

        // remove duplicates
        var permissions = request.Permissions
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        // map to entity
        var role = new ApplicationRole
        {
            Name = request.Name,
            NormalizedName = request.Name.ToUpper(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };

        try
        {
            // create role + permissions via repository
            var createdRole = await roleRepository.AddAsync(role, permissions);

            var response = new RoleDetailResponse
            {
                Id = createdRole.Id,
                Name = createdRole.Name ?? string.Empty,
                IsDeleted = createdRole.IsDeleted,
                Permissions = permissions
            };

            return Result.Success(response);
        }
        catch (Exception)
        {
            return Result.Failure<RoleDetailResponse>(RoleErrors.CreationFailed);
        }
    }
    // Update role + sync permissions
    public async Task<Result> UpdateAsync(string id, RoleReqest request)
    {
        try
        {
            var role = await roleRepository.UpdateAsync(id, request.Name, request.Permissions);
            if (role == null)
                return Result.Failure(RoleErrors.NotFound);

            return Result.Success();
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("DuplicatedRole"))
                return Result.Failure(RoleErrors.DuplicatedRole);

            return Result.Failure(RoleErrors.UpdateFailed);
        }
    }

    // Toggle IsDeleted status
    public async Task<Result> ToggleStatusAsync(string id)
    {
        var role = await roleRepository.ToggleStatusAsync(id);
        if (role == null)
            return Result.Failure(RoleErrors.NotFound);

        return Result.Success(role);
    }


}
