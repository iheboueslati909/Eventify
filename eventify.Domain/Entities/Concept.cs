using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;

namespace eventify.Domain.Entities;

public class Concept
{
    public Guid Id { get; private set; }
    public Title Name { get; private set; }
    public Description Description { get; private set; }
    public IReadOnlyCollection<MusicGenre> Genres => _genres.AsReadOnly();
    private readonly List<MusicGenre> _genres = new();

    private Concept() { } // Required for EF Core

    public Concept(string name, string description, List<MusicGenre> genres)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Concept name is required.");
        if (genres == null || genres.Count == 0) throw new ArgumentException("At least one genre must be selected.");

        Name = name;
        Description = description;
        _genres = genres;
    }

    public void AddGenre(MusicGenre genre)
    {
        if (genre == null) throw new ArgumentNullException(nameof(genre));
        if (_genres.Contains(genre)) throw new InvalidOperationException("Genre already exists.");
        _genres.Add(genre);
    }

    public void RemoveGenre(MusicGenre genre)
    {
        if (genre == null) throw new ArgumentNullException(nameof(genre));
        if (!_genres.Contains(genre)) throw new InvalidOperationException("Genre not found.");
        _genres.Remove(genre);
    }
}
