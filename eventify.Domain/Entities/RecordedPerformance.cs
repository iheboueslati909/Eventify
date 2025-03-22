using eventify.Domain.Enums;
using System;

namespace eventify.Domain.Entities;

public class RecordedPerformance
{
    public int Id { get; private set; }
    public int EventId { get; private set; }
    public Event Event { get; private set; }

    // New: One-to-One Relationship with TimeTableSlot
    public int TimeTableSlotId { get; private set; }
    public TimeTableSlot TimeTableSlot { get; private set; }

    public string RecordingLink { get; private set; }
    public DateTime Timestamp { get; private set; }
    public PerformanceType Type { get; private set; }

    private RecordedPerformance() { } // Required for EF Core

    public RecordedPerformance(int eventId, int timeTableSlotId, string recordingLink)
    {
        if (string.IsNullOrWhiteSpace(recordingLink))
            throw new ArgumentException("Recording link is required.");

        EventId = eventId;
        TimeTableSlotId = timeTableSlotId;
        RecordingLink = recordingLink;
        Timestamp = DateTime.UtcNow;
    }
}
