using eventify.Domain.Entities;
using eventify.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eventify.Infrastructure.Persistence.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.OwnsOne(e => e.Title, t =>
        {
            t.Property(tt => tt.Value)
                .HasColumnName("Title")
                .IsRequired();
        });

        builder.OwnsOne(e => e.Description, d =>
        {
            d.Property(dd => dd.Value)
                .HasColumnName("Description");
        });

        // Map StartDate and EndDate
        builder.Property(e => e.StartDate)
            .HasColumnName("StartDate")
            .IsRequired();

        builder.Property(e => e.EndDate)
            .HasColumnName("EndDate")
            .IsRequired();

        builder.Property(e => e.Type)
            .HasConversion<string>();

        builder.Property(e => e.Status)
            .HasConversion<string>();

        builder.OwnsOne(e => e.Location, loc =>
        {
            loc.Property(l => l.Address)
               .HasColumnName("Location")
               .IsRequired();
        });

        builder.HasOne<Concept>()
            .WithMany()
            .HasForeignKey(e => e.ConceptId)
            .OnDelete(DeleteBehavior.Restrict);
 
        builder.HasIndex(e => e.ConceptId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => new { e.StartDate, e.EndDate });
    }
    
}
