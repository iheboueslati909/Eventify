using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;

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
    public static Member Create(Name firstName, Name lastName, Email email, Password password)
    {
        if (firstName == null) throw new ArgumentNullException(nameof(firstName));
        if (lastName == null) throw new ArgumentNullException(nameof(lastName));
        if (email == null) throw new ArgumentNullException(nameof(email));
        if (password == null) throw new ArgumentNullException(nameof(password));

        return new Member(firstName, lastName, email, password);
    }

    public ArtistProfile CreateArtistProfile(Name artistName,Email email, SocialMediaLinks socialMediaLinks, MusicGenreCollection genres)
    {
        if (_artistProfiles.Any(p => p.ArtistName == artistName))
            throw new InvalidOperationException("Artist profile with this name already exists.");

        var profile = new ArtistProfile(
            Id,
            artistName,
            email,
            socialMediaLinks,
            genres.Genres);
            
        _artistProfiles.Add(profile);
        return profile;
    }

    public void UpdateInformation(Name firstName, Name lastName, Email email)
    {
        if (firstName == null) throw new ArgumentNullException(nameof(firstName));
        if (lastName == null) throw new ArgumentNullException(nameof(lastName));
        if (email == null) throw new ArgumentNullException(nameof(email));

        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
    
    public void SoftDelete()
    {
        IsDeleted = true;
    }

}
