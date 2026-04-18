using MalaebBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.EntitiesConfigurations;
public class ScheduleRuleConfiguration : IEntityTypeConfiguration<ScheduleRule>
{
    public void Configure(EntityTypeBuilder<ScheduleRule> builder)
    {
        builder.HasKey(s => s.Id);
        // ربط الـ Rule بالملعب
        builder.HasOne(s => s.Stadium)
               .WithMany() // الملعب ممكن يكون ليه أكتر من Rule (مثلا rule للصيف وrule للشتا)
               .HasForeignKey(s => s.StadiumId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}