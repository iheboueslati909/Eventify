using eventify.Domain.Entities;
using eventify.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eventify.Infrastructure.Persistence.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.OwnsOne(m => m.Email, e =>
        {
            e.Property(em => em.Value)
                .HasColumnName("Email")
                .IsRequired();
        });

        builder.OwnsOne(m => m.Password, p =>
        {
            p.Property(pp => pp.Hash)
                .HasColumnName("Password")
                .IsRequired();
        });

        builder.OwnsOne(m => m.FirstName, fn =>
        {
            fn.Property(f => f.Value)
                .HasColumnName("FirstName")
                .IsRequired();
        });

        builder.OwnsOne(m => m.LastName, ln =>
        {
            ln.Property(l => l.Value)
                .HasColumnName("LastName")
                .IsRequired();
        });
    }
}
