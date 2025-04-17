using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;
using eventify.SharedKernel;

namespace eventify.Domain.Entities;

public class ArtistProfile
{
    public Guid Id { get; private set; }
    public Guid MemberId { get; private set; }
    public Name ArtistName { get; private set; }
    public Bio Bio { get; private set; }
    public bool IsDeleted { get; private set; } = false;
    private MusicGenreCollection _genres = MusicGenreCollection.Empty;
    public IReadOnlyCollection<MusicGenre> Genres => _genres.Genres;
    public Email Email { get; private set; }
    public SocialMediaLinks SocialMediaLinks { get; private set; }

    private ArtistProfile() { } // Required for EF Core

    private ArtistProfile(
        Guid memberId,
        Name artistName,
        Email email,
        SocialMediaLinks socialMediaLinks,
        IEnumerable<MusicGenre> genres)
    {
        Id = Guid.NewGuid();
        MemberId = memberId;
        ArtistName = artistName;
        Email = email;
        SocialMediaLinks = socialMediaLinks;
        _genres = new MusicGenreCollection(genres);
    }

    public static Result<ArtistProfile> Create(
        Guid memberId,
        Name artistName,
        Email email,
        SocialMediaLinks socialMediaLinks,
        IEnumerable<MusicGenre> genres)
    {
        if (memberId == Guid.Empty) return Result.Failure<ArtistProfile>("Member ID cannot be empty");
        if (artistName == null) return Result.Failure<ArtistProfile>("Artist name cannot be null");
        if (email == null) return Result.Failure<ArtistProfile>("Email cannot be null");
        if (socialMediaLinks == null) return Result.Failure<ArtistProfile>("Social media links cannot be null");
        if (genres == null) return Result.Failure<ArtistProfile>("Genres collection cannot be null");

        return Result.Success(new ArtistProfile(memberId, artistName, email, socialMediaLinks, genres));
    }

    public Result UpdateInformation(
        Name artistName,
        Email email,
        SocialMediaLinks socialMediaLinks,
        IEnumerable<MusicGenre> genres)
    {
        if (artistName == null) return Result.Failure("Artist name cannot be null");
        if (email == null) return Result.Failure("Email cannot be null");
        if (socialMediaLinks == null) return Result.Failure("Social media links cannot be null");
        if (genres == null) return Result.Failure("Genres collection cannot be null");

        ArtistName = artistName;
        Email = email;
        SocialMediaLinks = socialMediaLinks;
        _genres = new MusicGenreCollection(genres);
        return Result.Success();
    }
    
    public void SoftDelete()
    {
        IsDeleted = true;
    }
}
