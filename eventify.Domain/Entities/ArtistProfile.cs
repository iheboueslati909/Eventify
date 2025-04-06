using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;

namespace eventify.Domain.Entities;

public class ArtistProfile
{
    public Guid Id { get; private set; }
    public Guid MemberId { get; private set; } // The owner of the profile
    public Name ArtistName { get; private set; }
    public Bio Bio { get; private set; }
    public IReadOnlyCollection<MusicGenre> Genres => _genres.AsReadOnly();
    public Email Email { get; private set; }
    public SocialMediaLinks SocialMediaLinks { get; private set; }

    private readonly List<MusicGenre> _genres = new();

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
        Bio = newBio;
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

    public void AddGenre(MusicGenre genre)
    {
        if (genre == null) throw new ArgumentNullException(nameof(genre));
        if (!_genres.Contains(genre))
            _genres.Add(genre);
    }

    public void RemoveGenre(MusicGenre genre)
    {
        if (genre == null) throw new ArgumentNullException(nameof(genre));
        _genres.Remove(genre);
    }

    public void ReplaceGenres(IEnumerable<MusicGenre> newGenres)
    {
        if (newGenres == null) throw new ArgumentNullException(nameof(newGenres));
        
        _genres.Clear();
        _genres.AddRange(newGenres.Where(g => g != null).Distinct());
    }
}