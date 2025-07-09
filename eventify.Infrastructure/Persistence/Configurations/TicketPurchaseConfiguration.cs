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

        // TicketPurchase - Ticket (many-to-one)
        builder.HasOne(tp => tp.Ticket)
            .WithMany(t => t.TicketPurchases)
            .HasForeignKey(tp => tp.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        // TicketPurchase - Member (User) (many-to-one)
        builder.HasOne(tp => tp.User)
            .WithMany(m => m.TicketPurchases)
            .HasForeignKey(tp => tp.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}