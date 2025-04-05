namespace eventify.Domain.Entities;

public class TimeTable
{
    public Guid Id { get; private set; }

    private TimeTable() { } // Required for EF Core

    public TimeTable()
    {
        Id = Guid.NewGuid();
    }
}
