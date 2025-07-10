using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;
using eventify.SharedKernel;

namespace eventify.Application.Events.Commands;

public record CreateEventCommand(
    string Title,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    Location Location,
    EventType Type,
    Guid ConceptId,
    IEnumerable<TimeTableCreationDto> TimeTables) : ICommand<Result<Guid>>;

public record TimeTableCreationDto(
    string StageName,
    IEnumerable<TimeTableSlotDto> Slots);

public record TimeTableSlotDto(
    DateTime StartTime,
    DateTime EndTime,
    string Title,
    IEnumerable<Guid> ArtistIds);
