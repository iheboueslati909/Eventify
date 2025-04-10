using eventify.Domain.Entities;
using eventify.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eventify.Infrastructure.Persistence.Configurations;

public class TimeTableConfiguration : IEntityTypeConfiguration<TimeTable>
{
    public void Configure(EntityTypeBuilder<TimeTable> builder)
    {
        builder.HasMany(t => t.Slots)
               .WithOne()
               .HasForeignKey("TimeTableId")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Event>()
               .WithMany()
               .HasForeignKey("EventId")
               .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(ap => ap.StageName, an =>
        {
            an.Property(a => a.Value)
                .HasColumnName("StageName")
                .IsRequired();
        });
        
    }
}
