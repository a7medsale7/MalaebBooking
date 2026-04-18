using MalaebBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.EntitiesConfigurations;
public class TimeSlotConfiguration : IEntityTypeConfiguration<TimeSlot>
{
    public void Configure(EntityTypeBuilder<TimeSlot> builder)
    {
        builder.HasKey(t => t.Id);
        // العلاقات
        builder.HasOne(t => t.Stadium)
               .WithMany(s => s.TimeSlots)
               .HasForeignKey(t => t.StadiumId)
               .OnDelete(DeleteBehavior.Cascade);
        // منع الحجوزات المزدوجة فى نفس الوقت لنفس الملعب
        builder.HasIndex(t => new { t.StadiumId, t.Date, t.StartTime }).IsUnique();

        // ربط الـ TimeSlot بالـ ScheduleRule اللي ولّدها (لو موجودة)
        builder.HasOne(t => t.ScheduleRule)
               .WithMany(sr => sr.GeneratedTimeSlots)
               .HasForeignKey(t => t.ScheduleRuleId)
               .OnDelete(DeleteBehavior.SetNull); // لو مسحنا القاعدة، المواعيد المحجوزة منها متتمسحش، بس تفقد الارتباط

    }
}