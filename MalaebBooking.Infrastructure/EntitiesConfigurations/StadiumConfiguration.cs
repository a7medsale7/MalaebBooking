using MalaebBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.EntitiesConfigurations;
public class StadiumConfiguration : IEntityTypeConfiguration<Stadium>
{
    public void Configure(EntityTypeBuilder<Stadium> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Description).HasMaxLength(1000);
        builder.Property(s => s.Address).HasMaxLength(500).IsRequired();
        builder.Property(s => s.GoogleMapsUrl).HasMaxLength(500);

        // دايماً الفلوس بنخلي نوعها (مبلغ دقيق) عشان ميهربش كسور
        builder.Property(s => s.PricePerHourDay).HasColumnType("decimal(18,2)");
        builder.Property(s => s.PricePerHourNight).HasColumnType("decimal(18,2)");


        builder.Property(s => s.PhoneNumber).HasMaxLength(20).IsRequired();
        builder.Property(s => s.InstapayNumber).HasMaxLength(50);
        // العلاقات (Relationships)
        // 1. الملعب بيتبع رياضة واحدة
        builder.HasOne(s => s.SportType)
               .WithMany(st => st.Stadiums)
               .HasForeignKey(s => s.SportTypeId)
               .OnDelete(DeleteBehavior.Restrict);
        // 2. الملعب بيملكه يوزر واحد (Owner)
        // جوا ميثود Configure في ملف StadiumConfiguration.cs
        // استبدل الجزء القديم بتاع المالك بده:

        builder.HasOne(s => s.OwnerProfile)
               .WithMany(p => p.Stadiums)
               .HasForeignKey(s => s.OwnerProfileId)
               .OnDelete(DeleteBehavior.Restrict);

    }
}