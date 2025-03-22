using System;

namespace eventify.Domain.Entities;

public class TimeTableSlot
{
    public int Id { get; private set; }
    public int EventId { get; private set; }
    public Event Event { get; private set; } // Navigation Property

    public int ArtistAccountId { get; private set; }  // Who is performing
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }

    // One-to-One Relationship with RecordedPerformance
    public RecordedPerformance? RecordedPerformance { get; private set; }

    private TimeTableSlot() { } // Required for EF Core

    public TimeTableSlot(int eventId, int artistAccountId, DateTime startTime, DateTime endTime)
    {
        if (startTime >= endTime)
            throw new ArgumentException("Start time must be before end time.");

        EventId = eventId;
        ArtistAccountId = artistAccountId;
        StartTime = startTime;
        EndTime = endTime;
    }
}
