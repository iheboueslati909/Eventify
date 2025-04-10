using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;

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

    public ArtistProfile(
        Guid memberId,
        Name artistName,
        Email email,
        SocialMediaLinks socialMediaLinks,
        IEnumerable<MusicGenre> genres)
    {
        UpdateMemberId(memberId);
        UpdateArtistName(artistName);
        UpdateEmail(email);
        UpdateSocialMediaLinks(socialMediaLinks);
        ReplaceGenres(genres);
    }

    public void UpdateArtistName(Name newName)
    {
        ArtistName = newName ?? throw new ArgumentNullException(nameof(newName));
    }

    public void UpdateBio(Bio newBio)
    {
        Bio = newBio; // Optional, so no null check
    }

    public void UpdateEmail(Email newEmail)
    {
        Email = newEmail ?? throw new ArgumentNullException(nameof(newEmail));
    }

    public void UpdateSocialMediaLinks(SocialMediaLinks newLinks)
    {
        SocialMediaLinks = newLinks ?? throw new ArgumentNullException(nameof(newLinks));
    }

    public void UpdateMemberId(Guid memberId)
    {
        if (memberId == Guid.Empty)
            throw new ArgumentException("Member ID cannot be empty", nameof(memberId));

        MemberId = memberId;
    }

    public void ReplaceGenres(IEnumerable<MusicGenre> newGenres)
    {
        _genres = new MusicGenreCollection(newGenres);
    }

    public void AddGenre(MusicGenre genre)
    {
        _genres = _genres.Add(genre);
    }

    public void RemoveGenre(MusicGenre genre)
    {
        _genres = _genres.Remove(genre);
    }
    
    public void SoftDelete()
    {
        IsDeleted = true;
    }
}
