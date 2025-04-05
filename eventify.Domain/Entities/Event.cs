using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;
using System;

namespace eventify.Domain.Entities;

public class Event
{
    public Guid Id { get; private set; }
    public Title Name { get; private set; } // Changed from Title to Name for clarity
    public Description Description { get; private set; }
    public DateRange DateRange { get; private set; } // Renamed from Date to DateRange
    public EventType Type { get; private set; }
    public EventStatus Status { get; private set; } = EventStatus.Draft;

    private Event() { } // Required for EF Core

    public Event(Title name, Description description, DateRange dateRange, EventType type)
    {
        UpdateName(name);
        UpdateDescription(description);
        Reschedule(dateRange);
        ChangeType(type);
    }

    // Domain-intent methods
    public void UpdateName(Title newName)
    {
        Name = newName ?? throw new ArgumentNullException(nameof(newName));
    }

    public void UpdateDescription(Description newDescription)
    {
        Description = newDescription; // Allowing null if description is optional
    }

    public void Reschedule(DateRange newDateRange)
    {
        DateRange = newDateRange ?? throw new ArgumentNullException(nameof(newDateRange));
        
        if (Status != EventStatus.Draft && DateTime.UtcNow > newDateRange.Start)
            throw new InvalidOperationException("Cannot reschedule active or past events");
    }

    public void ChangeType(EventType newType)
    {
        Type = newType ?? throw new ArgumentNullException(nameof(newType));
    }

    // Status transition methods
    public void Publish()
    {
        if (Status != EventStatus.Draft)
            throw new InvalidOperationException("Only draft events can be published");

        Status = EventStatus.Published;
    }

    public void Cancel()
    {
        if (Status == EventStatus.Canceled)
            throw new InvalidOperationException("Event is already canceled");
        if (Status == EventStatus.Completed)
            throw new InvalidOperationException("Cannot cancel a completed event");

        Status = EventStatus.Canceled;
    }

    public void Complete()
    {
        if (Status != EventStatus.Published)
            throw new InvalidOperationException("Only published events can be completed");
        if (DateTime.UtcNow < DateRange.End)
            throw new InvalidOperationException("Cannot complete event before end date");

        Status = EventStatus.Completed;
    }

    public void Reopen()
    {
        if (Status != EventStatus.Canceled)
            throw new InvalidOperationException("Only canceled events can be reopened");

        Status = EventStatus.Draft;
    }
}