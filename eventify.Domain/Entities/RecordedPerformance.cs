using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;

namespace eventify.Domain.Entities;

public class RecordedPerformance
{
    public Guid Id { get; private set; }
    public Url MediaUrl { get; private set; }
    public PerformanceType Type { get; private set; }
    public DateRange Timestamp { get; private set; }

    private RecordedPerformance() { }

    public RecordedPerformance(Url mediaUrl, PerformanceType type, DateTime performanceStart, DateTime performanceEnd)
    {
        MediaUrl = mediaUrl;
        Type = type;
        Timestamp = new DateRange(performanceStart, performanceEnd);
    }

    public void UpdateMediaUrl(Url newUrl)
    {
        MediaUrl = newUrl;
    }

    public void UpdatePerformanceTime(DateTime newStart, DateTime newEnd)
    {
        Timestamp = new DateRange(newStart, newEnd);
    }
}