using eventify.Domain.Entities;
using eventify.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eventify.Infrastructure.Persistence.Configurations;

public class TimeTableConfiguration : IEntityTypeConfiguration<Timetable>
{
    public void Configure(EntityTypeBuilder<Timetable> builder)
    {
        builder.HasOne<Event>()
               .WithMany(e => e.Timetables)
               .HasForeignKey(t => t.EventId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(ap => ap.StageName, an =>
        {
            an.Property(a => a.Value)
                .HasColumnName("StageName")
                .IsRequired();
        });
        
        builder.HasMany(t => t.Slots)
            .WithOne()
            .HasForeignKey(s => s.TimetableId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}
