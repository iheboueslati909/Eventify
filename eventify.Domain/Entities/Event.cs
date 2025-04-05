using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;

namespace eventify.Domain.Entities;

public class Event
{
    public Guid Id { get; private set; }
    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public DateRange Date { get; private set; }
    public EventType Type { get; private set; }

    private Event() { } // Required for EF Core

    public Event(Title title, Description description, DateRange date, EventType type)
    {
        Title = title;
        Description = description;
        Date = date;
        Type = type;
    }
}
