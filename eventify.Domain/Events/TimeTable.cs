using eventify.Domain.ValueObjects;
using eventify.SharedKernel;

namespace eventify.Domain.Entities;

public class TimeTable
{
    public Guid Id { get; private set; }
    public Guid EventId { get; private set; }
    public Title StageName { get; private set; }
    private readonly List<TimeTableSlot> _slots = new();
    public IReadOnlyCollection<TimeTableSlot> Slots => _slots.AsReadOnly();

    private TimeTable() { } // EF Core

    private TimeTable(Title stageName, List<TimeTableSlot> initialSlots, Guid eventId)
    {
        if (initialSlots == null || !initialSlots.Any())
            throw new ArgumentException("TimeTable must have at least one slot");

        Id = Guid.NewGuid();
        StageName = stageName;
        _slots.AddRange(initialSlots);
        EventId = eventId;
    }

    public static Result<TimeTable> Create(Title stageName, List<TimeTableSlot> initialSlots, Guid eventId)
    {
        if (stageName == null)
            return Result.Failure<TimeTable>("Stage name cannot be null");
            
        if (initialSlots == null || !initialSlots.Any())
            return Result.Failure<TimeTable>("TimeTable must have at least one slot");

        return Result.Success(new TimeTable(stageName, initialSlots, eventId));
    }

    public Result UpdateTimeTableSlots(List<TimeTableSlot> newSlots)
    {// TODO: Enforce no overlapping slots or duplicate artists
        if (newSlots == null || !newSlots.Any())
            return Result.Failure("TimeTable must have at least one slot");

        _slots.Clear();
        _slots.AddRange(newSlots);
        return Result.Success();
    }
}