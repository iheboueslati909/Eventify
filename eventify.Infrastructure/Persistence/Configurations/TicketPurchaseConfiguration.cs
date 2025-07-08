using eventify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eventify.Infrastructure.Persistence.Configurations;

public class TicketPurchaseConfiguration : IEntityTypeConfiguration<TicketPurchase>
{
    public void Configure(EntityTypeBuilder<TicketPurchase> builder)
    {
        builder.HasKey(tp => tp.Id);

        builder.Property(tp => tp.TicketId)
            .IsRequired();

        builder.Property(tp => tp.UserId)
            .IsRequired();

        builder.Property(tp => tp.Status)
            .IsRequired();

        builder.Property(tp => tp.PurchasedAt)
            .IsRequired();

        builder.Property(tp => tp.PaymentId)
            .IsRequired(false);

        // One-to-one with Ticket
        builder.HasOne(tp => tp.Ticket)
            .WithOne()
            .HasForeignKey<TicketPurchase>(tp => tp.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-one with Member (User)
        builder.HasOne(tp => tp.User)
            .WithOne()
            .HasForeignKey<TicketPurchase>(tp => tp.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}