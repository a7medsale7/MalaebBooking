using MalaebBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.EntitiesConfigurations;
public class StadiumImageConfiguration : IEntityTypeConfiguration<StadiumImage>
{
    public void Configure(EntityTypeBuilder<StadiumImage> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.ImageUrl).HasMaxLength(500).IsRequired();
        // ربط الصورة بالملعب (لو الملعب اتمسح صورته تتمسح معاه = Cascade)
        builder.HasOne(i => i.Stadium)
               .WithMany(s => s.Images)
               .HasForeignKey(i => i.StadiumId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}