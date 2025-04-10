using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace eventify.Domain.Entities;

//aggregate root

public class Event
{
    public Guid Id { get; private set; }
    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public Location Location { get; private set; }
    public EventType Type { get; private set; }
    public EventStatus Status { get; private set; } = EventStatus.Draft;
    public Guid ConceptId { get; private set; }
    public bool IsDeleted { get; private set; } = false;

    private readonly List<TimeTable> _timeTables = new();
    public IReadOnlyCollection<TimeTable> TimeTables => _timeTables.AsReadOnly();

    private Event() { } // Required for EF Core

    private Event(Title title, Description description, DateTime startDate, DateTime endDate, Location location, EventType type, Guid conceptId)
    {
        Title = title;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        Location = location;
        Type = type;
        ConceptId = conceptId;

    }

    public static Event Create(Title title, Description description, DateTime startDate, DateTime endDate, Location location, EventType type,  Guid conceptId)
    {
        if (title == null) throw new ArgumentNullException(nameof(title));
        if (description == null) throw new ArgumentNullException(nameof(description));
        if (location == null) throw new ArgumentNullException(nameof(location));

        return new Event(title, description, startDate, endDate, location, type, conceptId);
    }   

    public void UpdateDetails(Title title, Description description, DateTime startDate, DateTime endDate, Location location, EventType type)
    {
        Title = title;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        Location = location;
        Type = type;
    }

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
        if (DateTime.UtcNow < EndDate)
            throw new InvalidOperationException("Cannot complete event before end date");

        Status = EventStatus.Completed;
    }

    public void Reopen()
    {
        if (Status != EventStatus.Canceled)
            throw new InvalidOperationException("Only canceled events can be reopened");

        Status = EventStatus.Draft;
    }

    public void AddTimeTable(TimeTable table)
    {
        _timeTables.Add(table);
    }

    public void SoftDelete()
    {
        if (Status == EventStatus.Published)
            throw new InvalidOperationException("Can't soft-delete published events.");

        IsDeleted = true;
    }
}