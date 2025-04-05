namespace eventify.Domain.Entities;

public class TimeTableSlot
{
    public Guid Id { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public String Title { get; private set; }

    private TimeTableSlot() { }

    public TimeTableSlot(TimeSpan startTime, TimeSpan endTime, String Title)
    {
        if (startTime >= endTime)
            throw new ArgumentException("Start time must be before end time.", nameof(startTime));

        Id = Guid.NewGuid();
        StartTime = startTime;
        EndTime = endTime;
    }
}