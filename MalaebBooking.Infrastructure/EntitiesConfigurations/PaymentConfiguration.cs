using MalaebBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.EntitiesConfigurations;
public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Amount).HasColumnType("decimal(18,2)");
        builder.Property(p => p.PaymentScreenshotUrl).HasMaxLength(500).IsRequired();
        builder.Property(p => p.PlayerPhoneNumber).HasMaxLength(20).IsRequired();
        builder.Property(p => p.RejectionReason).HasMaxLength(500);
        // الدفع بيكون لحجز واحد (One-to-One)
        builder.HasOne(p => p.Booking)
               .WithOne(b => b.Payment)
               .HasForeignKey<Payment>(p => p.BookingId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}