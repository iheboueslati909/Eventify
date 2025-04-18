using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;
using System;
using eventify.SharedKernel;

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

    public static Result<RecordedPerformance> Create(Url mediaUrl, PerformanceType type)
    {
        if (mediaUrl == null)
            return Result.Failure<RecordedPerformance>("Media URL cannot be null.");
        if (!Enum.IsDefined(typeof(PerformanceType), type))
            return Result.Failure<RecordedPerformance>("Invalid performance type.");

        return Result.Success(new RecordedPerformance(mediaUrl, type));
    }

    public Result UpdateMediaUrl(Url newUrl)
    {
        if (newUrl == null)
            return Result.Failure("Media URL cannot be null.");

        MediaUrl = newUrl;
        LastModified = DateTime.UtcNow;
        return Result.Success();
    }

    public Result ChangePerformanceType(PerformanceType newType)
    {
        if (!Enum.IsDefined(typeof(PerformanceType), newType))
            return Result.Failure("Invalid performance type.");

        Type = newType;
        LastModified = DateTime.UtcNow;
        return Result.Success();
    }

    public void UpdateMedia(Url newUrl, PerformanceType newType)
    {
        UpdateMediaUrl(newUrl);
        ChangePerformanceType(newType);
    }
}