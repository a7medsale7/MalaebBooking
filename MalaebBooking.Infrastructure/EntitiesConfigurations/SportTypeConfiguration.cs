using MalaebBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.EntitiesConfigurations;
public class SportTypeConfiguration : IEntityTypeConfiguration<SportType>
{
    public void Configure(EntityTypeBuilder<SportType> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name).HasMaxLength(100).IsRequired();
        builder.Property(s => s.Description).HasMaxLength(500);
        builder.Property(s => s.IconUrl).HasMaxLength(500);
        builder.HasIndex(s => s.Name).IsUnique(); // مفيش رياضتين بنفس الاسم
    }
}