using MalaebBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.EntitiesConfigurations;
public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.TotalPrice).HasColumnType("decimal(18,2)");
        builder.Property(b => b.CancellationReason).HasMaxLength(500);
        // اللاعب بيعمل حجز (لو اللاعب اتمسح الحجز يفضل موجود للتاريخ)
        builder.HasOne(b => b.Player)
               .WithMany(u => u.Bookings)
               .HasForeignKey(b => b.PlayerId)
               .OnDelete(DeleteBehavior.Restrict);
        // كل وقت بيكون له حجز واحد بس بالكتير (One-to-One)
        builder.HasOne(b => b.TimeSlot)
               .WithOne(t => t.Booking)
               .HasForeignKey<Booking>(b => b.TimeSlotId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}