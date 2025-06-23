using System.Text.Json.Serialization;
namespace eventify.Domain.ValueObjects;
using eventify.SharedKernel;

public class SocialMediaLinks
{
    public Url? SoundCloud { get; }
    public Url? Spotify { get; }
    public Url? Facebook { get; }
    public Url? Instagram { get; }
    public Url? Youtube { get; }

    public SocialMediaLinks() { } // For deserialization

    [JsonConstructor]
    public SocialMediaLinks(Url? soundCloud, Url? spotify, Url? facebook, Url? instagram, Url? youtube)
    {
        SoundCloud = soundCloud;
        Spotify = spotify;
        Facebook = facebook;
        Instagram = instagram;
        Youtube = youtube;
    }

    public static Result<SocialMediaLinks> Create(IEnumerable<string> links)
    {
        if (links == null)
            return Result.Failure<SocialMediaLinks>("Links collection cannot be null.");

        Url? soundCloud = null, spotify = null, facebook = null, instagram = null, youtube = null;

        foreach (var link in links)
        {
            if (string.IsNullOrWhiteSpace(link))
                continue;

            var urlResult = Url.Create(link);
            if (urlResult.IsFailure)
                return Result.Failure<SocialMediaLinks>($"Invalid URL: {link}");

            var lower = link.ToLowerInvariant();
            if (lower.Contains("soundcloud"))
                soundCloud = urlResult.Value;
            else if (lower.Contains("spotify"))
                spotify = urlResult.Value;
            else if (lower.Contains("facebook"))
                facebook = urlResult.Value;
            else if (lower.Contains("instagram"))
                instagram = urlResult.Value;
            else if (lower.Contains("youtube"))
                youtube = urlResult.Value;
        }

        return Result.Success(new SocialMediaLinks(soundCloud, spotify, facebook, instagram, youtube));
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
