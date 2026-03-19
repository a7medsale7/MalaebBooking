using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Consts;
using MalaebBooking.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public RoleRepository(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    // جلب كل الرولات
    public async Task<List<ApplicationRole>> GetAllAsync(bool? includeDisabled = false)
    {
        return await _roleManager.Roles
            .Where(x => !x.IsDefault && (!x.IsDeleted || (includeDisabled.HasValue && includeDisabled.Value)))
            .ToListAsync();
    }

    // جلب رول واحد بالـ id
    public async Task<ApplicationRole?> GetByIdAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        if (role == null || role.IsDeleted)
            return null;

        return role;
    }


    public async Task<ApplicationRole> AddAsync(ApplicationRole role, IEnumerable<string> permissions)
    {
        // create role
        var result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        // add permissions as claims
        foreach (var permission in permissions
            .Distinct(StringComparer.OrdinalIgnoreCase))
        {
            var claimResult = await _roleManager.AddClaimAsync(
                role,
                new Claim(Permissions.Type, permission)
            );

            if (!claimResult.Succeeded)
                throw new Exception(string.Join(", ", claimResult.Errors.Select(e => e.Description)));
        }

        return role;
    }

    public async Task<ApplicationRole?> UpdateAsync(string id, string newName, IEnumerable<string> newPermissions)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
            return null;

        // check duplicate name
        var exists = _roleManager.Roles.Any(x => x.Name == newName && x.Id != id);
        if (exists)
            throw new Exception("DuplicatedRole");

        // update role name
        role.Name = newName;
        role.NormalizedName = newName.ToUpper();

        var updateResult = await _roleManager.UpdateAsync(role);
        if (!updateResult.Succeeded)
            throw new Exception(updateResult.Errors.First().Description);

        // get current permissions
        var currentClaims = await _roleManager.GetClaimsAsync(role);
        var currentPermissions = currentClaims
            .Where(c => c.Type == Permissions.Type)
            .Select(c => c.Value)
            .ToList();

        // permissions to add
        var permissionsToAdd = newPermissions.Except(currentPermissions)
            .Distinct(StringComparer.OrdinalIgnoreCase);

        foreach (var permission in permissionsToAdd)
        {
            var addResult = await _roleManager.AddClaimAsync(role, new Claim(Permissions.Type, permission));
            if (!addResult.Succeeded)
                throw new Exception(addResult.Errors.First().Description);
        }

        // permissions to remove
        var permissionsToRemove = currentPermissions.Except(newPermissions);
        foreach (var permission in permissionsToRemove)
        {
            var claim = currentClaims.FirstOrDefault(c => c.Type == Permissions.Type && c.Value == permission);
            if (claim != null)
            {
                var removeResult = await _roleManager.RemoveClaimAsync(role, claim);
                if (!removeResult.Succeeded)
                    throw new Exception(removeResult.Errors.First().Description);
            }
        }

        return role;
    }

    // Toggle IsDeleted status
    public async Task<ApplicationRole?> ToggleStatusAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
            return null;

        role.IsDeleted = !role.IsDeleted;

        var updateResult = await _roleManager.UpdateAsync(role);
        if (!updateResult.Succeeded)
            throw new Exception(updateResult.Errors.First().Description);

        return role;
    }
}