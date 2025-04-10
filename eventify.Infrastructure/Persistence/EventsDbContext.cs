using eventify.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
    public DbSet<RecordedPerformance> RecordedPerformances { get; set; }
    public DbSet<ArtistProfile> ArtistProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Concept>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<Event>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Member>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ArtistProfile>().HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventsDbContext).Assembly);
    }
}
