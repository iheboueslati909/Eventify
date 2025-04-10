using eventify.Domain.Entities;
using eventify.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eventify.Infrastructure.Persistence.Converters;

namespace eventify.Infrastructure.Persistence.Configurations;

public class ConceptConfiguration : IEntityTypeConfiguration<Concept>
{
    public void Configure(EntityTypeBuilder<Concept> builder)
    {
        builder.Property(typeof(MusicGenreCollection), "_genres")
            .HasConversion(MusicGenreCollectionConverter.ToStringConverter)
            .HasField("_genres")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Genres")
            .HasColumnType("varchar(255)");

        // Map Name (Value Object: Title)
        builder.OwnsOne(c => c.Title, n =>
        {
            n.Property(nn => nn.Value)
             .HasColumnName("Title")
             .IsRequired();
        });

        // Map Description (Value Object: Description)
        builder.OwnsOne(c => c.Description, d =>
        {
            d.Property(dd => dd.Value)
             .HasColumnName("Description");
        });

        // Map relationship with Member
        builder.HasOne<Member>()
            .WithMany()
            .HasForeignKey(t => t.MemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
