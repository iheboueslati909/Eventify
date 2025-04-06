using eventify.Domain.Entities;
using eventify.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace eventify.Infrastructure.Persistence;

public class EventsDbContext : DbContext
{
    public EventsDbContext(DbContextOptions<EventsDbContext> options) : base(options) { }
    public EventsDbContext() { }

    public DbSet<Event> Events { get; set; }
    public DbSet<TimeTable> TimeTables { get; set; }
    public DbSet<TimeTableSlot> TimeTableSlots { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Concept> Concepts { get; set; }
    public DbSet<MemberEvent> MemberEvents { get; set; }
    public DbSet<RecordedPerformance> RecordedPerformances { get; set; }
    public DbSet<ArtistProfile> ArtistProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Event → TimeTable (One-to-Many)
        modelBuilder.Entity<TimeTable>()
            .HasOne<Event>()
            .WithMany()
            .HasForeignKey("EventId")
            .OnDelete(DeleteBehavior.Cascade);

        // TimeTable → TimeTableSlot (One-to-Many)
        modelBuilder.Entity<TimeTable>()
            .HasMany(t => t.Slots)
            .WithOne()
            .HasForeignKey("TimeTableId")
            .OnDelete(DeleteBehavior.Cascade);

        // TimeTableSlot → RecordedPerformance (One-to-One)
        modelBuilder.Entity<RecordedPerformance>()
            .HasOne<TimeTableSlot>()
            .WithOne()
            .HasForeignKey<RecordedPerformance>("TimeTableSlotId")
            .OnDelete(DeleteBehavior.Cascade);

        // Member → Concept (One-to-Many)
        modelBuilder.Entity<Concept>()
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey("MemberId")
            .OnDelete(DeleteBehavior.Cascade);

        // Member → MemberEvent (One-to-Many)
        modelBuilder.Entity<MemberEvent>()
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey("MemberId")
            .OnDelete(DeleteBehavior.Cascade);

        // Event → MemberEvent (One-to-Many)
        modelBuilder.Entity<MemberEvent>()
            .HasOne<Event>()
            .WithMany()
            .HasForeignKey("EventId")
            .OnDelete(DeleteBehavior.Cascade);

        // ArtistProfile → Member (One-to-One)
        modelBuilder.Entity<ArtistProfile>()
            .HasOne<Member>()
            .WithOne()
            .HasForeignKey<ArtistProfile>("MemberId")
            .OnDelete(DeleteBehavior.Cascade);

        // Store Enums as Strings (Optional)
        modelBuilder.Entity<Event>()
            .Property(e => e.Type)
            .HasConversion<string>();

        // Value Object Mappings
        modelBuilder.Entity<Event>()
            .OwnsOne(e => e.DateRange, dr =>
            {
                dr.Property(d => d.Start).HasColumnName("DateRange_Start");
                dr.Property(d => d.End).HasColumnName("DateRange_End");
            });

        modelBuilder.Entity<Event>()
            .OwnsOne(e => e.Title, t =>
            {
                t.Property(tt => tt.Value).HasColumnName("Title_Value");
            });

        modelBuilder.Entity<Member>()
            .OwnsOne(m => m.Email, e =>
            {
                e.Property(em => em.Value).HasColumnName("Email");
            });

        modelBuilder.Entity<Club>()
            .OwnsOne(c => c.Location, l =>
            {
                l.Property(loc => loc.Address).HasColumnName("Location_Address");
            });

        // Concept → MusicGenreCollection as comma-separated integers
        var musicGenreConverter = new ValueConverter<MusicGenreCollection, string>(
            v => v.ToString(),
            v => MusicGenreCollection.FromString(v)
        );

        modelBuilder.Entity<Concept>()
            .Property(typeof(MusicGenreCollection), "_musicGenres")
            .HasConversion(musicGenreConverter)
            .HasColumnName("MusicGenres")
            .HasColumnType("varchar(255)");

    }
}