using MalaebBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.EntitiesConfigurations;
public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        // المفتاح الأساسي
        builder.HasKey(r => r.Id);

        // خصائص
        builder.Property(r => r.Comment)
               .HasMaxLength(1000);

        // علاقة التقييم بالملعب
        builder.HasOne(r => r.Stadium)
               .WithMany(s => s.Reviews)
               .HasForeignKey(r => r.StadiumId)
               .OnDelete(DeleteBehavior.Cascade);

        // علاقة التقييم باللاعب (لازم يبقى موجود حتى لو الحساب اتحذف)
        builder.HasOne(r => r.Player)
               .WithMany(u => u.Reviews)
               .HasForeignKey(r => r.PlayerId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}