using eventify.Domain.Entities;
using eventify.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using eventify.Infrastructure.Persistence.Converters;

namespace eventify.Infrastructure.Persistence.Configurations;

public class ArtistProfileConfiguration : IEntityTypeConfiguration<ArtistProfile>
{
    public void Configure(EntityTypeBuilder<ArtistProfile> builder)
    {
        builder.Property(typeof(MusicGenreCollection), "_genres")
            .HasConversion(MusicGenreCollectionConverter.ToStringConverter)
            .HasField("_genres")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Genres")
            .HasColumnType("varchar(255)");


        builder.OwnsOne(ap => ap.Email, e =>
        {
            e.Property(em => em.Value)
                .HasColumnName("Email");
        });

        builder.OwnsOne(ap => ap.Bio, b =>
        {
            b.Property(bb => bb.Value)
                .HasColumnName("Bio");
        });

        builder.OwnsOne(ap => ap.SocialMediaLinks, sm =>
        {
            sm.Property(s => s.SoundCloud)
                .HasConversion(
                    url => url.Value,
                    s => Url.FromDatabase(s)) // ✅ Hydrate without validation
                .HasColumnName("SoundCloud");

            sm.Property(s => s.Spotify)
                .HasConversion(
                    url => url.Value,
                    s => Url.FromDatabase(s)) // ✅ Clean and consistent
                .HasColumnName("Spotify");

            sm.Property(s => s.Facebook)
                .HasConversion(
                    url => url.Value,
                    s => Url.FromDatabase(s))
                .HasColumnName("Facebook");

            sm.Property(s => s.Instagram)
                .HasConversion(
                    url => url.Value,
                    s => Url.FromDatabase(s))
                .HasColumnName("Instagram");

            sm.Property(s => s.Youtube)
                .HasConversion(
                    url => url.Value,
                    s => Url.FromDatabase(s))
                .HasColumnName("Youtube");
        });


        builder.OwnsOne(ap => ap.ArtistName, an =>
        {
            an.Property(a => a.Value)
                .HasColumnName("ArtistName")
                .IsRequired();
        });

        builder.HasOne<Member>()
            .WithMany(m => m.ArtistProfiles)
            .HasForeignKey(t => t.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
