using eventify.Domain.Enums;

namespace eventify.Domain.ValueObjects;

public class MusicGenreCollection
{
    private readonly List<MusicGenre> _genres;

    public IReadOnlyCollection<MusicGenre> Genres => _genres.AsReadOnly();

    private MusicGenreCollection() { } // Required for EF Core
    
    public MusicGenreCollection(IEnumerable<MusicGenre> genres)
    {
        if (genres == null || !genres.Any())
            throw new ArgumentException("At least one music genre must be specified.");

        _genres = new List<MusicGenre>(genres);
    }

    public void AddGenre(MusicGenre genre)
    {
        if (_genres.Contains(genre))
            throw new InvalidOperationException("Genre already exists.");

        _genres.Add(genre);
    }

    public void RemoveGenre(MusicGenre genre)
    {
        if (!_genres.Contains(genre))
            throw new InvalidOperationException("Genre not found.");

        _genres.Remove(genre);
    }
}
