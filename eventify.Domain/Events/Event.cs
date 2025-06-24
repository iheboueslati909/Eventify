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

    public Guid? ClubId { get; private set; } // Optional Club reference

    private readonly List<Timetable> _timetables = new();
    public IReadOnlyCollection<Timetable> Timetables => _timetables.AsReadOnly();

    private Event() { } // Required for EF Core

    private Event(
        Title title,
        Description description,
        DateTime startDate,
        DateTime endDate,
        Location location,
        EventType type,
        Guid conceptId,
        List<Timetable> timetables,
        Guid? clubId = null)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        Location = location;
        Type = type;
        ConceptId = conceptId;
        ClubId = clubId;
        if (timetables != null)
            _timetables.AddRange(timetables);

        foreach (var timetable in _timetables)
        {
            timetable.SetEventId(Id);
            foreach (var slot in timetable.Slots)
            {
                slot.SetTimeTableId(timetable.Id);
            }
        }
    }

    public static Result<Event> Create(
        Title title,
        Description description,
        DateTime startDate,
        DateTime endDate,
        Location location,
        EventType type,
        Guid conceptId,
        IEnumerable<(Title StageName, IEnumerable<(TimeSpan StartTime, TimeSpan EndTime, Title SlotTitle, IEnumerable<ArtistProfile> ArtistProfiles)> Slots)> timetables,
        Guid? clubId = null)
    {
        if (startDate >= endDate)
            return Result.Failure<Event>("End date must be after start date");

        if (startDate < DateTime.UtcNow)
            return Result.Failure<Event>("Start date cannot be in the past");

        if (title == null) throw new ArgumentNullException(nameof(title));
        if (description == null) throw new ArgumentNullException(nameof(description));
        if (location == null) throw new ArgumentNullException(nameof(location));
        if (timetables == null)
            return Result.Failure<Event>("At least one timetable is required.");

        var timetableEntities = new List<Timetable>();

        foreach (var (stageName, slots) in timetables)
        {
            if (stageName == null)
                return Result.Failure<Event>("Stage name cannot be null.");

            var slotEntities = new List<TimeTableSlot>();
            foreach (var (startTime, endTime, slotTitle, artistProfiles) in slots)
            {
                if (slotTitle == null)
                    return Result.Failure<Event>("Slot title cannot be null.");

                var slotResult = TimeTableSlot.Create(Guid.Empty, startTime, endTime, slotTitle);
                if (slotResult.IsFailure)
                    return Result.Failure<Event>(slotResult.Error);

                var slot = slotResult.Value;
                foreach (var artistProfile in artistProfiles ?? Array.Empty<ArtistProfile>())
                {
                    slot.AssignArtist(artistProfile);
                }
                slotEntities.Add(slot);
            }

            var timetableResult = Timetable.Create(stageName, slotEntities, Guid.Empty);
            if (timetableResult.IsFailure)
                return Result.Failure<Event>(timetableResult.Error);

            timetableEntities.Add(timetableResult.Value);
        }

        var @event = new Event(title, description, startDate, endDate, location, type, conceptId, timetableEntities, clubId);

        return Result.Success(@event);
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

    public void AddTimetable(Timetable timetable)
    {
        _timetables.Add(timetable);
    }

    public void SoftDelete()
    {
        if (Status == EventStatus.Published)
            throw new InvalidOperationException("Can't soft-delete published events.");

        IsDeleted = true;
    }
}