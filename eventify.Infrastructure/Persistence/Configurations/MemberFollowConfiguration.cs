
using Eventify.Domain.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace eventify.Infrastructure.Persistence.Configurations;

public class MemberFollowConfiguration : IEntityTypeConfiguration<MemberFollow>
{
    public void Configure(EntityTypeBuilder<MemberFollow> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.MemberId)
               .IsRequired();

        builder.Property(f => f.TargetId)
               .IsRequired();

        builder.Property(f => f.TargetType)
               .HasConversion<int>()
               .IsRequired();

        builder.Property(f => f.CreatedAt)
               .IsRequired();

        builder.Property(f => f.IsMuted)
               .IsRequired();

        builder.Property(f => f.NotificationType)
               .HasConversion<int>()
               .IsRequired();

        builder.HasIndex(f => new { f.MemberId, f.TargetId, f.TargetType })
               .IsUnique(); // enforce 1 follow per target
    }
}
