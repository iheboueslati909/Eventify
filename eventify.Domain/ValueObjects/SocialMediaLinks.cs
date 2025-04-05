namespace eventify.Domain.ValueObjects;

public class SocialMediaLinks
{
    public Url? SoundCloud { get; }
    public Url? Spotify { get; }
    public Url? Facebook { get; }
    public Url? Instagram { get; }
    public Url? Youtube { get; }

    public SocialMediaLinks(Url? soundCloud, Url? spotify, Url? facebook, Url? instagram, Url? youtube)
    {
        SoundCloud = soundCloud;
        Spotify = spotify;
        Facebook = facebook;
        Instagram = instagram;
        Youtube = youtube;
    }
}
