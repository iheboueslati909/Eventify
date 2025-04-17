using System;
using eventify.SharedKernel;

namespace eventify.Domain.ValueObjects;

public class DateRange
{
    public DateTime Start { get; }
    public DateTime End { get; }

    private DateRange() { } // Required for EF Core

    private DateRange(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    public static Result<DateRange> Create(DateTime start, DateTime end)
    {
        if (start >= end)
            return Result.Failure<DateRange>("Start time must be before end time");

        return Result.Success(new DateRange(start, end));
    }

    // Optional practical methods
    public TimeSpan Duration => End - Start;

    public bool Contains(DateTime time) => time >= Start && time <= End;

    public override string ToString() => $"{Start:t} - {End:t}"; // Time-only format

    public override bool Equals(object? obj) =>
        obj is DateRange range && 
        Start == range.Start && 
        End == range.End;

    public override int GetHashCode() => HashCode.Combine(Start, End);
}