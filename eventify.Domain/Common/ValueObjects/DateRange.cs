using System;

namespace eventify.Domain.ValueObjects;

public class DateRange
{
    public DateTime Start { get; }
    public DateTime End { get; }

    private DateRange() { } // Required for EF Core

    public DateRange(DateTime start, DateTime end)
    {
        if (start >= end)
            throw new ArgumentException("Start time must be before end time");

        Start = start;
        End = end;
    }

    // Optional practical methods
    public TimeSpan Duration => End - Start;

    public bool Contains(DateTime time) => time >= Start && time <= End;

    public override string ToString() => $"{Start:t} - {End:t}"; // Time-only format
}