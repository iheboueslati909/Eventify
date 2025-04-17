namespace eventify.Domain.ValueObjects;
using eventify.SharedKernel;

public class SocialMediaLinks
{
    public Url? SoundCloud { get; }
    public Url? Spotify { get; }
    public Url? Facebook { get; }
    public Url? Instagram { get; }
    public Url? Youtube { get; }

    private SocialMediaLinks() { } // Required for EF Core

    public SocialMediaLinks(Url? soundCloud, Url? spotify, Url? facebook, Url? instagram, Url? youtube)
    {
        SoundCloud = soundCloud;
        Spotify = spotify;
        Facebook = facebook;
        Instagram = instagram;
        Youtube = youtube;
    }

    public override bool Equals(object? obj) =>
        obj is SocialMediaLinks links &&
        EqualityComparer<Url?>.Default.Equals(SoundCloud, links.SoundCloud) &&
        EqualityComparer<Url?>.Default.Equals(Spotify, links.Spotify) &&
        EqualityComparer<Url?>.Default.Equals(Facebook, links.Facebook) &&
        EqualityComparer<Url?>.Default.Equals(Instagram, links.Instagram) &&
        EqualityComparer<Url?>.Default.Equals(Youtube, links.Youtube);

    public override int GetHashCode() => HashCode.Combine(SoundCloud, Spotify, Facebook, Instagram, Youtube);
}
