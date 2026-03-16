using MalaebBooking.Domain.Consts;
using MalaebBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.EntitiesConfigurations;
internal class ApplicationRolesConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData([
            new ApplicationRole
            {
                Id = DefaultRoles.AdminRoleId,
                Name = DefaultRoles.Admin,
                NormalizedName = DefaultRoles.Admin.ToUpper(),
                ConcurrencyStamp = DefaultRoles.AdminRoleConcurrencyStamp,
            },

            new ApplicationRole
            {
                Id = DefaultRoles.PlayerRoleId,
                Name = DefaultRoles.Player,
                NormalizedName = DefaultRoles.Player.ToUpper(),
                ConcurrencyStamp = DefaultRoles.PlayerRoleConcurrencyStamp,
                IsDefault = true,
            },

             new ApplicationRole
            {
                Id = DefaultRoles.OwnerRoleId,
                Name = DefaultRoles.Owner,
                NormalizedName = DefaultRoles.Owner.ToUpper(),
                ConcurrencyStamp = DefaultRoles.OwnerRoleConcurrencyStamp,
            },

            ]);
    }
}
