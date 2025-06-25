using eventify.Application.Common;
using eventify.Application.Repositories;
using eventify.Domain.Entities;
using eventify.Domain.ValueObjects;
using eventify.SharedKernel;

namespace eventify.Application.Clubs.Commands;

public record CreateClubCommand(
    string Name,
    string Address,
    int Capacity,
    IEnumerable<Guid> OwnerMemberIds
) : ICommand<Result<Guid>>;

public class CreateClubCommandHandler : ICommandHandler<CreateClubCommand, Result<Guid>>
{
    private readonly IClubRepository _clubRepository;
    private readonly IMemberRepository _memberRepository;

    public CreateClubCommandHandler(IClubRepository clubRepository, IMemberRepository memberRepository)
    {
        _clubRepository = clubRepository;
        _memberRepository = memberRepository;
    }

    public async Task<Result<Guid>> Handle(CreateClubCommand command, CancellationToken cancellationToken)
    {
        var nameResult = Name.Create(command.Name);
        if (nameResult.IsFailure)
            return Result.Failure<Guid>(nameResult.Error);

        var addressResult = Location.Create(command.Address);
        if (addressResult.IsFailure)
            return Result.Failure<Guid>(addressResult.Error);

        if (command.Capacity <= 0)
            return Result.Failure<Guid>("Capacity must be positive.");

        if (command.OwnerMemberIds == null || !command.OwnerMemberIds.Any())
            return Result.Failure<Guid>("At least one owner is required.");
        var ownerMembers = _memberRepository.GetByIdsAsync(command.OwnerMemberIds.ToList(), cancellationToken).Result;
        if (ownerMembers == null || !ownerMembers.Any())
            return Result.Failure<Guid>("At least one valid owner member is required.");

        var clubResult = Club.Create(
            nameResult.Value,
            addressResult.Value,
            command.Capacity,
            ownerMembers
        );

        if (clubResult.IsFailure)
            return Result.Failure<Guid>(clubResult.Error);

        await _clubRepository.AddAsync(clubResult.Value);
        await _clubRepository.SaveChangesAsync();

        return Result.Success(clubResult.Value.Id);
    }
}
