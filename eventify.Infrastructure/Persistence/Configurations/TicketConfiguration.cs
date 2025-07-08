using eventify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eventify.Infrastructure.Persistence.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.EventId)
            .IsRequired();

        builder.Property(t => t.CreatorId)
            .IsRequired();

        builder.Property(t => t.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(t => t.Quantity)
            .IsRequired();

        builder.Property(t => t.ReservedCount)
            .IsRequired();

        builder.Property(t => t.Currency)
            .HasConversion<string>()
            .IsRequired();

        builder.HasIndex(t => t.EventId);
        builder.HasIndex(t => t.CreatorId);

        builder.HasOne(t => t.Event)
            .WithMany(e => e.Tickets)
            .HasForeignKey(t => t.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.Creator)
            .WithMany(m => m.Tickets)
            .HasForeignKey(t => t.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(t => t.Name, n =>
        {
            n.Property(x => x.Value)
                .HasColumnName("Name")
                .IsRequired();
        });
    }
}
