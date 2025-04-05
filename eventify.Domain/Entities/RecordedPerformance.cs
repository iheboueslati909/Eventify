using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;
using System;

namespace eventify.Domain.Entities;

public class RecordedPerformance
{
    public Guid Id { get; private set; }
    public Url MediaUrl { get; private set; }
    public PerformanceType Type { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastModified { get; private set; }

    private RecordedPerformance() { } // Required for EF Core

    public RecordedPerformance(Url mediaUrl, PerformanceType type)
    {
        UpdateMediaUrl(mediaUrl);
        ChangePerformanceType(type);
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateMediaUrl(Url newUrl)
    {
        MediaUrl = newUrl ?? throw new ArgumentNullException(nameof(newUrl));
        LastModified = DateTime.UtcNow;
    }

    public void ChangePerformanceType(PerformanceType newType)
    {
        if (!Enum.IsDefined(typeof(PerformanceType), newType))
            throw new ArgumentException("Invalid performance type", nameof(newType));

        Type = newType;
        LastModified = DateTime.UtcNow;
    }

    // Optional: Media metadata update method
    public void UpdateMedia(Url newUrl, PerformanceType newType)
    {
        UpdateMediaUrl(newUrl);
        ChangePerformanceType(newType);
    }
}