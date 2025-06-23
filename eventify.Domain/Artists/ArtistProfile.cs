using System;
using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;
using eventify.SharedKernel;
public class ArtistProfile
{
    public Guid Id { get; private set; }
    public Guid MemberId { get; private set; }
    public Name ArtistName { get; private set; }
    public Bio? Bio { get; private set; }
    public bool IsDeleted { get; private set; } = false;
    private MusicGenreCollection _genres;

    public IReadOnlyCollection<MusicGenre> Genres => _genres.Genres;
    public Email Email { get; private set; }
    public SocialMediaLinks? SocialMediaLinks { get; private set; }

    private ArtistProfile() { } // Required for EF Core

    private ArtistProfile(
        Guid memberId,
        Name artistName,
        Email email,
        Bio bio,
        MusicGenreCollection genres,
        SocialMediaLinks? socialMediaLinks
        )
    {
        Id = Guid.NewGuid();
        MemberId = memberId;
        ArtistName = artistName;
        Email = email;
        Bio = bio;
        SocialMediaLinks = socialMediaLinks;
        _genres = genres;
    }

    public static Result<ArtistProfile> Create(
        Guid memberId,
        Name artistName,
        Email email,
        Bio bio,
        SocialMediaLinks socialMediaLinks,
        MusicGenreCollection genres)
    {
        return Result.Success(new ArtistProfile(memberId, artistName, email, bio, genres, socialMediaLinks));
    }

    public Result UpdateInformation(
        Name artistName,
        Email email,
        Bio bio,
        IEnumerable<MusicGenre> genres,
        SocialMediaLinks socialMediaLinks
        )
    {
        var genreResult = MusicGenreCollection.Create(genres);
        if (!genreResult.IsSuccess)
            return Result.Failure<ArtistProfile>(genreResult.Error);

        ArtistName = artistName;
        Email = email;
        Bio = bio;
        SocialMediaLinks = socialMediaLinks;
        _genres = genreResult.Value;
        return Result.Success();
    }
    
    public void SoftDelete()
    {
        IsDeleted = true;
    }
}
