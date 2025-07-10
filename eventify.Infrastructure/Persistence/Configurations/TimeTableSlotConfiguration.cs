using eventify.Domain.Entities;
using eventify.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eventify.Infrastructure.Persistence.Configurations;

public class TimeTableSlotConfiguration : IEntityTypeConfiguration<TimeTableSlot>
{
    public void Configure(EntityTypeBuilder<TimeTableSlot> builder)
    {
        builder.HasOne<Timetable>()
               .WithMany(t => t.Slots)
               .HasForeignKey(tt => tt.TimetableId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(t => t.StartTime)
            .IsRequired();

        builder.Property(t => t.EndTime)
            .IsRequired();

        builder.OwnsOne(t => t.Title, tt =>
        {
            tt.Property(t => t.Value)
              .HasColumnName("Title")
              .IsRequired();
        });

        builder.HasMany(s => s.ArtistProfiles)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "TimeTableSlotArtistProfiles",
                j => j.HasOne<ArtistProfile>().WithMany().HasForeignKey("ArtistProfileId"),
                j => j.HasOne<TimeTableSlot>().WithMany().HasForeignKey("TimeTableSlotId"));
    }
}
