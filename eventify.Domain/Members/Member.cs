using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;
using eventify.SharedKernel;
namespace eventify.Domain.Entities;

//aggregate root
public class Member
{
    public Guid Id { get; private set; }
    public Name FirstName { get; private set; }
    public Name LastName { get; private set; }
    public Email Email { get; private set; }
    public Password Password { get; private set; }

    public bool IsDeleted { get; private set; } = false;
    private readonly List<ArtistProfile> _artistProfiles = new();
    public IReadOnlyCollection<ArtistProfile> ArtistProfiles => _artistProfiles.AsReadOnly();
    private Member() { }

    private Member(Name firstName, Name lastName, Email email, Password password)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
    }
    public static Result<Member> Create(Name firstName, Name lastName, Email email, Password password)
    {
        return Result.Success(new Member(firstName, lastName, email, password));
    }

    public Result<ArtistProfile> CreateArtistProfile(Name artistName, Email email, Bio bio, SocialMediaLinks socialMediaLinks, MusicGenreCollection genres)
    {
        if (_artistProfiles.Any(p => p.ArtistName == artistName))
            return Result.Failure<ArtistProfile>("Artist profile with this name already exists.");

        var profileResult = ArtistProfile.Create(
            Id,
            artistName,
            email,
            bio,
            socialMediaLinks,
            genres.Genres
        );

        if (profileResult.IsSuccess)
        {
            _artistProfiles.Add(profileResult.Value);
            return Result.Success(profileResult.Value);
        }
        return Result.Failure<ArtistProfile>(profileResult.Error);
    }

    public Result UpdateInformation(Name firstName, Name lastName, Email email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        return Result.Success();
    }
    
    public Result SoftDelete()
    {
        if (_artistProfiles.Any(p => !p.IsDeleted))
            return Result.Failure("Cannot delete member with active artist profiles");

        IsDeleted = true;
        return Result.Success();
    }

}
