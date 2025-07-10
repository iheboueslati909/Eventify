using System;
using System.Collections.Generic;
using eventify.Domain.ValueObjects;
using eventify.SharedKernel;


public class TimeTableSlot
{
    public Guid Id { get; private set; }
    public Guid TimetableId { get; private set; } // Use only this property
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public Title Title { get; private set; }
    private readonly List<ArtistProfile> _artistProfiles = new();
    public IReadOnlyCollection<ArtistProfile> ArtistProfiles => _artistProfiles.AsReadOnly();


    private TimeTableSlot() { }

    private TimeTableSlot(Guid timetableId, DateTime StartTime, DateTime EndTime, Title title)
    {
        if (StartTime >= EndTime)
            throw new ArgumentException("StartTime must be before EndTime.", nameof(StartTime));

        Id = Guid.NewGuid();
        TimetableId = timetableId;
        StartTime = StartTime;
        EndTime = EndTime;
        Title = title;
    }

    public static Result<TimeTableSlot> Create(Guid timetableId, DateTime StartTime, DateTime EndTime, Title title)
    {
        if (StartTime >= EndTime)
            return Result.Failure<TimeTableSlot>("StartTime must be before EndTime");

        if (title == null)
            return Result.Failure<TimeTableSlot>("Title cannot be null");

        return Result.Success(new TimeTableSlot(timetableId, StartTime, EndTime, title));
    }

    internal void SetTimeTableId(Guid timetableId)
    {
        TimetableId = timetableId;
    }

    public void AssignArtist(ArtistProfile artist)
    {
        _artistProfiles.Add(artist);
    }
}