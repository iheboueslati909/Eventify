using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;
using EventsManagement.Domain.ValueObjects;

namespace eventify.Domain.Entities;
public class ArtistProfile
{
    public Guid Id { get; private set; }
    public Guid MemberId { get; private set; } // The owner of the profile
    public Name FirstName { get; private set; }
    public Bio Bio { get; private set; } = string.Empty;
    public MusicGenre[] Genres { get; private set; } = Array.Empty<MusicGenre>();

    public Email Email { get; private set; }
    public SocialMediaLinks SocialMediaLinks { get; private set; }

    private ArtistProfile() { } // Required for EF Core

    public ArtistProfile(Guid memberId, string name, string bio, MusicGenre[] genre, SocialMediaLinks socialMediaLinks)
    {
        MemberId = memberId;
        Name = name;
        Bio = bio;
        Genres = genre;
        SocialMediaLinks = socialMediaLinks;
    }
}
