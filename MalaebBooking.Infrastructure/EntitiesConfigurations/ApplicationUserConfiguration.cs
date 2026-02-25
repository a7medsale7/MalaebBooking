using MalaebBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.EntitiesConfigurations;
public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(u => u.LastName).HasMaxLength(100).IsRequired();
        // تظبيط הـ RefreshTokens عشان يعملهم في جدول منفصل مرتبط باليوزر
        builder.OwnsMany(u => u.RefreshTokens, t =>
        {
            t.ToTable("RefreshTokens");
            t.Property(r => r.Token).HasMaxLength(200).IsRequired();
            t.WithOwner().HasForeignKey("UserId"); // ربط التوكن باليوزر
        });
    }
}