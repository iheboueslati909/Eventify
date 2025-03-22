using eventify.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace eventify.Infrastructure;

public class EventsDbContext : DbContext
{
    public EventsDbContext(DbContextOptions<EventsDbContext> options) : base(options) { }

    public DbSet<Event> Events { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Club> Clubs { get; set; }
    public DbSet<BookingInvitation> BookingInvitations { get; set; }
    public DbSet<TimeTableSlot> TimeTableSlots { get; set; }
    public DbSet<RecordedPerformance> RecordedPerformances { get; set; }
    public DbSet<NewsFeedItem> NewsFeedItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Event → Club (Many-to-One)
        modelBuilder.Entity<Event>()
            .HasOne(e => e.Club)
            .WithMany(c => c.Events)
            .HasForeignKey(e => e.ClubId)
            .OnDelete(DeleteBehavior.Cascade);

        // Event → TimeTableSlot (One-to-Many)
        modelBuilder.Entity<Event>()
            .HasMany(e => e.TimeTableSlots)
            .WithOne(t => t.Event)
            .HasForeignKey(t => t.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        // Event → BookingInvitation (One-to-Many)
        modelBuilder.Entity<Event>()
            .HasMany(e => e.BookingInvitations)
            .WithOne(b => b.Event)
            .HasForeignKey(b => b.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        // Event → RecordedPerformance (One-to-Many)
        modelBuilder.Entity<Event>()
            .HasMany(e => e.RecordedPerformances)
            .WithOne(r => r.Event)
            .HasForeignKey(r => r.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        // TimeTableSlot → RecordedPerformance (One-to-One)
        modelBuilder.Entity<TimeTableSlot>()
            .HasOne(t => t.RecordedPerformance)
            .WithOne(r => r.TimeTableSlot)
            .HasForeignKey<RecordedPerformance>(r => r.TimeTableSlotId)
            .OnDelete(DeleteBehavior.Cascade);

        // BookingInvitation → Member (Many-to-One)
        modelBuilder.Entity<BookingInvitation>()
            .HasOne(b => b.Member)
            .WithMany(m => m.BookingInvitations)
            .HasForeignKey(b => b.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        // NewsFeedItem → Member (Many-to-One)
        modelBuilder.Entity<NewsFeedItem>()
            .HasOne(n => n.Member)
            .WithMany()
            .HasForeignKey(n => n.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        // Store Enums as Strings (Optional)
        modelBuilder.Entity<BookingInvitation>()
            .Property(b => b.Status)
            .HasConversion<string>();

        modelBuilder.Entity<RecordedPerformance>()
            .Property(r => r.Type)
            .HasConversion<string>();

        modelBuilder.Entity<Event>()
            .Property(e => e.Type)
            .HasConversion<string>();

        modelBuilder.Entity<Member>()
            .OwnsOne(m => m.Email, e =>
            {
                e.Property(e => e.Value)
                 .HasColumnName("Email")
                 .IsRequired();
            });

        modelBuilder.Entity<Event>()
            .OwnsOne(e => e.DateRange);

        modelBuilder.Entity<Event>()
            .OwnsOne(e => e.Title);
    }
}
