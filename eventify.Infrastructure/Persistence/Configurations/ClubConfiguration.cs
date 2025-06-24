using eventify.Domain.Entities;
using eventify.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eventify.Infrastructure.Persistence.Configurations;

public class ClubConfiguration : IEntityTypeConfiguration<Club>
{
    public void Configure(EntityTypeBuilder<Club> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasConversion(
                v => v.Value,
                v => new Name(v))
            .IsRequired();

        builder.Property(c => c.Address)
            .HasConversion(
                v => v.Address,
                v => new Location(v))
            .IsRequired();

        builder.Property(c => c.Capacity)
            .IsRequired();

        builder.Property(c => c.IsDeleted)
            .HasDefaultValue(false);

        // Many-to-many: Club <-> Member (OwnerMemberIds)
        builder
            .HasMany<Member>()
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "ClubOwner",
                j => j
                    .HasOne<Member>()
                    .WithMany()
                    .HasForeignKey("MemberId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Club>()
                    .WithMany()
                    .HasForeignKey("ClubId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("ClubId", "MemberId");
                    j.ToTable("ClubOwners");
                });

        // Backing field for OwnerMemberIds
        builder
            .Property(typeof(List<Guid>), "_ownerMemberIds")
            .HasField("_ownerMemberIds")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
