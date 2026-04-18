using MalaebBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.EntitiesConfigurations;
public class StadiumOwnerProfileConfiguration : IEntityTypeConfiguration<StadiumOwnerProfile>
{
    public void Configure(EntityTypeBuilder<StadiumOwnerProfile> builder)
    {
        builder.HasKey(x => x.Id);

        // 1. علاقة 1 لـ 1 مع اليوزر
        builder.HasOne(x => x.User)
               .WithOne(u => u.StadiumOwnerProfile)
               .HasForeignKey<StadiumOwnerProfile>(x => x.UserId);

        // 2. علاقة الملاعب مع البروفايل (One-to-Many)
        builder.HasMany(x => x.Stadiums)
               .WithOne(s => s.OwnerProfile)
               .HasForeignKey(s => s.OwnerProfileId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ApprovedBy)
               .WithMany()
               .HasForeignKey(x => x.ApprovedById);
    }
}
