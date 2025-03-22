using System;
using System.Collections.Generic;
using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;

namespace eventify.Domain.Entities;

public class Event
{
    public int Id { get; private set; }
    public EventTitle Title { get; private set; }
    public string Description { get; private set; }
    public DateRange DateRange { get; private set; }
    public int ClubId { get; private set; }
    public Club Club { get; private set; }  // Navigation Property

    // New: One-to-Many Relationship with TimeTableSlot
    public List<TimeTableSlot> TimeTableSlots { get; private set; } = new();

    // Existing Relationships
    public List<BookingInvitation> BookingInvitations { get; private set; } = new();
    public List<RecordedPerformance> RecordedPerformances { get; private set; } = new();
    public EventType Type { get; private set; }

    private Event() { } // Required for EF Core

    public Event(EventTitle title, string description, DateRange dateRange, int clubId)
    {
        Title = title;
        Description = description;
        DateRange = dateRange;
        ClubId = clubId;
    }
}
