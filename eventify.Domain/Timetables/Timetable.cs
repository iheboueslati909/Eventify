using System;
using System.Collections.Generic;
using System.Linq;
using eventify.Domain.ValueObjects;
using eventify.SharedKernel;


public class Timetable
{
    public Guid Id { get; private set; }
    public Guid EventId { get; private set; }
    public Title StageName { get; private set; }
    private readonly List<TimeTableSlot> _slots = new();
    public IReadOnlyCollection<TimeTableSlot> Slots => _slots.AsReadOnly();

    private Timetable() { } // EF Core

    private Timetable(Title stageName, List<TimeTableSlot> initialSlots, Guid eventId)
    {
        if (initialSlots == null || !initialSlots.Any())
            throw new ArgumentException("Timetable must have at least one slot");

        Id = Guid.NewGuid();
        StageName = stageName;
        _slots.AddRange(initialSlots);
        EventId = eventId;
    }

    public static Result<Timetable> Create(Title stageName, List<TimeTableSlot> initialSlots, Guid eventId)
    {
        if (stageName == null)
            return Result.Failure<Timetable>("Stage name cannot be null");
            
        if (initialSlots == null || !initialSlots.Any())
            return Result.Failure<Timetable>("Timetable must have at least one slot");

        return Result.Success(new Timetable(stageName, initialSlots, eventId));
    }

    public Result UpdateTimetableSlots(List<TimeTableSlot> newSlots)
    {// TODO: Enforce no overlapping slots or duplicate artists
        if (newSlots == null || !newSlots.Any())
            return Result.Failure("Timetable must have at least one slot");

        _slots.Clear();
        _slots.AddRange(newSlots);
        return Result.Success();
    }

    internal void SetEventId(Guid eventId)
    {
        EventId = eventId;
    }

    public void AddSlot(TimeTableSlot slot)
    {
        if (!_slots.Contains(slot))
            _slots.Add(slot);
    }
}