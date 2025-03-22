using System;

namespace eventify.Domain.ValueObjects;

public class EventTitle
{
    public string Value { get; private set; }

    private EventTitle() { } // Required for EF Core

    public EventTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.");

        if (title.Length > 100)
            throw new ArgumentException("Title is too long.");

        Value = title;
    }

    public override string ToString() => Value;
}
