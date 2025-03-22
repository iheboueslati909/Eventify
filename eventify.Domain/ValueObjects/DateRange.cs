using System;

namespace eventify.Domain.ValueObjects;

public class DateRange
{
    public DateTime Start { get; private set; }
    public DateTime End { get; private set; }

    private DateRange() { } // Required for EF Core

    public DateRange(DateTime start, DateTime end)
    {
        if (start >= end)
            throw new ArgumentException("Start date must be before the end date.");

        Start = start;
        End = end;
    }
}
