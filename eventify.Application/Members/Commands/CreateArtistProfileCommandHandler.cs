using eventify.Application.Common;
using eventify.Application.Repositories;
using eventify.Domain.ValueObjects;
using eventify.Domain.Entities;
using eventify.SharedKernel;

namespace eventify.Application.Members.Commands;

public record CreateArtistProfileCommand(
    Guid MemberId,
    string ArtistName,
    string Email,
    string Bio,
    List<string> SocialLinks,
    List<int> Genres
) : ICommand<Result<Guid>>;

public class CreateArtistProfileCommandHandler : ICommandHandler<CreateArtistProfileCommand, Result<Guid>>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IArtistProfileRepository _artistProfileRepository;

    public CreateArtistProfileCommandHandler(IMemberRepository memberRepository,
        IArtistProfileRepository artistProfileRepository)
    {
        _artistProfileRepository = artistProfileRepository;
        _memberRepository = memberRepository;
    }

    public async Task<Result<Guid>> Handle(CreateArtistProfileCommand command, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(command.MemberId);
        if (member == null)
            return Result.Failure<Guid>("Member not found");

        var artistNameResult = Name.Create(command.ArtistName);
        if (artistNameResult.IsFailure)
            return Result.Failure<Guid>(artistNameResult.Error);

        var emailResult = Email.Create(command.Email);
        if (emailResult.IsFailure)
            return Result.Failure<Guid>(emailResult.Error);

        var bioResult = Bio.Create(command.Bio);
        if (bioResult.IsFailure)
            return Result.Failure<Guid>(bioResult.Error);

        var socialLinksResult = SocialMediaLinks.Create(command.SocialLinks);
        if (socialLinksResult.IsFailure)
            return Result.Failure<Guid>(socialLinksResult.Error);

        var genres = new List<eventify.Domain.Enums.MusicGenre>();
        foreach (var genreValue in command.Genres)
        {
            if (!Enum.IsDefined(typeof(eventify.Domain.Enums.MusicGenre), genreValue))
                return Result.Failure<Guid>($"Invalid genre value: {genreValue}");
            genres.Add((eventify.Domain.Enums.MusicGenre)genreValue);
        }

        var genreCollectionResult = MusicGenreCollection.Create(genres);
        if (genreCollectionResult.IsFailure)
            return Result.Failure<Guid>(genreCollectionResult.Error);

        var artistProfileResult = ArtistProfile.Create(
            command.MemberId,
            artistNameResult.Value,
            emailResult.Value,
            bioResult.Value,
            socialLinksResult.Value,
            genreCollectionResult.Value
        );

        if (artistProfileResult.IsFailure)
            return Result.Failure<Guid>(artistProfileResult.Error);

        // You may want to add the artist profile to the repository if needed
        await _artistProfileRepository.AddAsync(artistProfileResult.Value);

        await _memberRepository.SaveChangesAsync();
        return Result.Success(artistProfileResult.Value.Id);
    }
}
