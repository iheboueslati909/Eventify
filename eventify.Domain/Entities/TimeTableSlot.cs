﻿namespace eventify.Domain.Entities;

public class TimeTableSlot
{
    public Guid Id { get; private set; }
    public Guid TimeTableId { get; private set; } // Reference to parent TimeTable
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public Title Title { get; private set; }

    private TimeTableSlot() { }

    public TimeTableSlot(Guid timeTableId, TimeSpan startTime, TimeSpan endTime, Title title)
    {
        if (startTime >= endTime)
            throw new ArgumentException("Start time must be before end time.", nameof(startTime));

        Id = Guid.NewGuid();
        TimeTableId = timeTableId;
        StartTime = startTime;
        EndTime = endTime;
        Title = title;
    }
}