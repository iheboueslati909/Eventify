using eventify.Domain.Entities;
using eventify.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eventify.Infrastructure.Persistence.Configurations;

public class TimeTableSlotConfiguration : IEntityTypeConfiguration<TimeTableSlot>
{
    public void Configure(EntityTypeBuilder<TimeTableSlot> builder)
    {
        builder.HasOne<TimeTable>()
               .WithMany(t => t.Slots)
               .HasForeignKey("TimeTableId")
               .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(t => t.Title, tt =>
        {
            tt.Property(t => t.Value)
              .HasColumnName("Title")
              .IsRequired();
        });
    }
}
