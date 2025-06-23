using eventify.Application.Common;
using eventify.Application.Common.Interfaces;
using eventify.Application.Repositories;
using eventify.Domain.Entities;
using eventify.Domain.ValueObjects;
using eventify.SharedKernel;

namespace eventify.Application.Events.Commands;

public class CreateEventCommandHandler  : ICommandHandler<CreateEventCommand, Result<Guid>>
{
    private readonly IEventRepository _eventRepository;
    private readonly ITimeTableSlotRepository _slotRepository;
    private readonly IArtistProfileRepository _artistProfileRepository; // Use dedicated repository

    public CreateEventCommandHandler(
        IEventRepository eventRepository,
        ITimeTableSlotRepository slotRepository,
        IArtistProfileRepository artistProfileRepository)
    {
        _eventRepository = eventRepository;
        _slotRepository = slotRepository;
        _artistProfileRepository = artistProfileRepository;
    }

    public async Task<Result<Guid>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        // Flatten all (artistId, slotDto) pairs for conflict checking
        var artistSlotPairs = request.TimeTables
            .SelectMany(timetable => timetable.Slots
                .SelectMany(slot => slot.ArtistIds.Select(artistId => (artistId, slot))))
            .ToList();

        foreach (var (artistId, slotDto) in artistSlotPairs)
        {
            var conflicts = await _slotRepository.GetConflictingSlotsForArtistAsync(
                artistId, slotDto.StartTime, cancellationToken);

            if (conflicts.Any(conflict =>
                (slotDto.StartTime < conflict.EndTime) && (slotDto.EndTime > conflict.StartTime)))
            {
                return Result.Failure<Guid>(
                    $"Artist {artistId} has a conflicting slot between {slotDto.StartTime} and {slotDto.EndTime}");
            }
        }

        var allArtistIds = request.TimeTables
            .SelectMany(t => t.Slots.SelectMany(s => s.ArtistIds))
            .Distinct()
            .ToList();

        var allArtistProfiles = await _artistProfileRepository
            .GetByIdsAsync(allArtistIds, CancellationToken.None);

        if (allArtistProfiles.Count != allArtistIds.Count)
            return Result.Failure<Guid>("Some artist profiles do not exist");

        var timetableTuples = request.TimeTables
            .Select(timetableDto =>
            {
                var stageTitleResult = Title.Create(timetableDto.StageName);
                if (stageTitleResult.IsFailure)
                    return (Error: stageTitleResult.Error, Value: default((Title, IEnumerable<(TimeSpan, TimeSpan, Title, IEnumerable<ArtistProfile>)>)));

                var slotTuples = new List<(TimeSpan, TimeSpan, Title, IEnumerable<ArtistProfile>)>();
                foreach (var slotDto in timetableDto.Slots)
                {
                    var slotTitleResult = Title.Create(slotDto.Title);
                    if (slotTitleResult.IsFailure)
                        return (Error: slotTitleResult.Error, Value: default((Title, IEnumerable<(TimeSpan, TimeSpan, Title, IEnumerable<ArtistProfile>)>)));

                    var artistProfiles = allArtistProfiles
                        .Where(ap => slotDto.ArtistIds.Contains(ap.Id))
                        .ToList();

                    slotTuples.Add((slotDto.StartTime, slotDto.EndTime, slotTitleResult.Value, artistProfiles));
                }
                return (Error: (string?)null, Value: (stageTitleResult.Value, slotTuples.AsEnumerable()));
            })
            .ToList();

        var error = timetableTuples.FirstOrDefault(t => t.Error != null).Error;
        if (error != null)
            return Result.Failure<Guid>(error);

        var eventResult = Event.Create(
            Title.Create(request.Title).Value,
            Description.Create(request.Description).Value,
            request.StartDate,
            request.EndDate,
            request.Location,
            request.Type,
            request.ConceptId,
            timetableTuples.Select(t => t.Value)
        );

        if (eventResult.IsFailure)
            return Result.Failure<Guid>(eventResult.Error);

        var @event = eventResult.Value;

        await _eventRepository.AddAsync(@event);
        await _eventRepository.SaveChangesAsync();

        return Result.Success(@event.Id);
    }
}

