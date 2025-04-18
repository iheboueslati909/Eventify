using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using eventify.SharedKernel;

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

    public static Result<Event> Create(Title title, Description description, DateTime startDate, DateTime endDate, Location location, EventType type, Guid conceptId)
    {
        if (startDate >= endDate)
            return Result.Failure<Event>("End date must be after start date");

        if (startDate < DateTime.UtcNow)
            return Result.Failure<Event>("Start date cannot be in the past");

        if (title == null) throw new ArgumentNullException(nameof(title));
        if (description == null) throw new ArgumentNullException(nameof(description));
        if (location == null) throw new ArgumentNullException(nameof(location));

        return Result.Success(new Event(title, description, startDate, endDate, location, type, conceptId));
    }   

    //isPublished
    public bool IsPublished => Status == EventStatus.Published;

    public void UpdateDetails(Title title, Description description, DateTime startDate, DateTime endDate, Location location, EventType type)
    {
        Title = title;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        Location = location;
        Type = type;
    }

    public Result Publish()
    {
        if (Status != EventStatus.Draft)
            return Result.Failure("Only draft events can be published");

        Status = EventStatus.Published;
        return Result.Success();
    }

    public Result Cancel()
    {
        if (Status == EventStatus.Canceled)
            return Result.Failure("Event is already canceled");
        if (Status == EventStatus.Completed)
            return Result.Failure("Cannot cancel a completed event");

        Status = EventStatus.Canceled;
        return Result.Success();
    }

    public Result Complete()
    {
        if (Status != EventStatus.Published)
            return Result.Failure("Only published events can be completed");
        if (DateTime.UtcNow < EndDate)
            return Result.Failure("Cannot complete event before end date");

        Status = EventStatus.Completed;
        return Result.Success();
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