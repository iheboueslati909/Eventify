namespace eventify.Domain.Entities;
using eventify.Domain.ValueObjects;
using eventify.SharedKernel;

public class TimeTableSlot
{
    public Guid Id { get; private set; }
    public Guid TimeTableId { get; private set; } // Reference to parent TimeTable
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public Title Title { get; private set; }
    private readonly List<Guid> _artistIds = new();
    public IReadOnlyCollection<Guid> ArtistIds => _artistIds.AsReadOnly();

    private TimeTableSlot() { }

    private TimeTableSlot(Guid timeTableId, TimeSpan startTime, TimeSpan endTime, Title title)
    {
        if (startTime >= endTime)
            throw new ArgumentException("Start time must be before end time.", nameof(startTime));

        Id = Guid.NewGuid();
        TimeTableId = timeTableId;
        StartTime = startTime;
        EndTime = endTime;
        Title = title;
    }

    public static Result<TimeTableSlot> Create(Guid timeTableId, TimeSpan startTime, TimeSpan endTime, Title title)
    {
        if (startTime >= endTime)
            return Result.Failure<TimeTableSlot>("Start time must be before end time");
            
        if (title == null)
            return Result.Failure<TimeTableSlot>("Title cannot be null");

        return Result.Success(new TimeTableSlot(timeTableId, startTime, endTime, title));
    }
}