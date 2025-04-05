namespace eventify.Domain.Aggregates;
using eventify.Domain.Entities;
using eventify.Domain.ValueObjects;
public class TimeTableAggregate
{
    public Guid Id { get; private set; }
    public Guid EventId { get; private set; }
    private readonly List<Guid> _slotIds = new();
    public IReadOnlyCollection<Guid> SlotIds => _slotIds.AsReadOnly();
    
    private TimeTableAggregate() { } // Required for EF Core

    public TimeTableAggregate(Guid eventId)
    {
        Id = Guid.NewGuid();
        EventId = eventId;
    }

    public void AddSlot(Guid slotId)
    {
        if (SlotIds.Contains(slotId))
            throw new InvalidOperationException("Slot is already associated with this time table.");

        SlotIds.Add(slotId);
    }

    public void RemoveSlot(Guid slotId)
    {
        if (!SlotIds.Contains(slotId))
            throw new InvalidOperationException("Slot is not associated with this time table.");

        SlotIds.Remove(slotId);
    }
}
