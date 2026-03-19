using MalaebBooking.Domain.Consts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.EntitiesConfigurations;
public class ApplicationRoleClaimConfigurations : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {
        var allPermissions = Permissions.GetAllPermissions();
        var allClaims = new List<IdentityRoleClaim<string>>();
        int claimId = 1;

        // 1. Admin Permissions (All)
        foreach (var permission in allPermissions)
        {
            if (string.IsNullOrEmpty(permission)) continue;
            allClaims.Add(new IdentityRoleClaim<string>
            {
                Id = claimId++,
                RoleId = DefaultRoles.AdminRoleId,
                ClaimType = Permissions.Type,
                ClaimValue = permission
            });
        }

        // 2. Owner Permissions
        var ownerPermissions = new List<string>
        {
            Permissions.Users_ViewProfile,
            Permissions.Users_UpdateProfile,
            Permissions.Users_ChangePassword,
            
            Permissions.Stadiums_View,
            Permissions.Stadiums_Create,
            Permissions.Stadiums_Update,
            Permissions.Stadiums_ToggleActive,
            
            Permissions.StadiumImages_View,
            Permissions.StadiumImages_Upload,
            Permissions.StadiumImages_Update,
            Permissions.StadiumImages_Delete,
            
            Permissions.TimeSlots_View,
            Permissions.TimeSlots_Create,
            Permissions.TimeSlots_Update,
            Permissions.TimeSlots_Delete,
            
            Permissions.Bookings_View,
            Permissions.Bookings_UpdateStatus,
            Permissions.Bookings_Cancel,
            
            Permissions.Payments_View,
            Permissions.Payments_Approve,
            Permissions.Payments_Reject,
            
            Permissions.Reviews_View
        };

        foreach (var permission in ownerPermissions)
        {
            allClaims.Add(new IdentityRoleClaim<string>
            {
                Id = claimId++,
                RoleId = DefaultRoles.OwnerRoleId,
                ClaimType = Permissions.Type,
                ClaimValue = permission
            });
        }

        builder.HasData(allClaims);
    }
}
