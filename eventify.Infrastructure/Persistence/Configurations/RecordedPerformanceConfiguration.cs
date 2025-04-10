using eventify.Domain.Entities;
using eventify.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eventify.Infrastructure.Persistence.Configurations;

public class RecordedPerformanceConfiguration : IEntityTypeConfiguration<RecordedPerformance>
{
    public void Configure(EntityTypeBuilder<RecordedPerformance> builder)
    {
        builder.Property(rp => rp.Type)
            .HasConversion<string>();

        builder.OwnsOne(rp => rp.MediaUrl, url =>
        {
            url.Property(u => u.Value)
               .HasColumnName("MediaUrl")
               .IsRequired();
        });

        builder.HasOne<TimeTableSlot>()
               .WithOne()
               .HasForeignKey<RecordedPerformance>("TimeTableSlotId")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
