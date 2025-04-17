using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;
using eventify.Domain.Common;
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
        if (firstName == null) return Result.Failure<Member>("First name cannot be null");
        if (lastName == null) return Result.Failure<Member>("Last name cannot be null");
        if (email == null) return Result.Failure<Member>("Email cannot be null");
        if (password == null) return Result.Failure<Member>("Password cannot be null");

        return Result.Success(new Member(firstName, lastName, email, password));
    }

    public Result<ArtistProfile> CreateArtistProfile(Name artistName, Email email, SocialMediaLinks socialMediaLinks, MusicGenreCollection genres)
    {
        if (_artistProfiles.Any(p => p.ArtistName == artistName))
            return Result.Failure<ArtistProfile>("Artist profile with this name already exists.");

        var profile = new ArtistProfile(
            Id,
            artistName,
            email,
            socialMediaLinks,
            genres.Genres);
            
        _artistProfiles.Add(profile);
        return Result.Success(profile);
    }

    public Result UpdateInformation(Name firstName, Name lastName, Email email)
    {
        if (firstName == null) return Result.Failure("First name cannot be null");
        if (lastName == null) return Result.Failure("Last name cannot be null");
        if (email == null) return Result.Failure("Email cannot be null");

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
