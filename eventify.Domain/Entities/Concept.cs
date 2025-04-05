using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace eventify.Domain.Entities;

public class Concept
{
    public Guid Id { get; private set; }
    public Title Name { get; private set; }
    public Description Description { get; private set; }
    public IReadOnlyCollection<MusicGenre> Genres => _genres.AsReadOnly();
    private readonly List<MusicGenre> _genres = new();

    private Concept() { } // Required for EF Core

    public Concept(Title name, Description description, IEnumerable<MusicGenre> genres)
    {
        UpdateName(name);
        UpdateDescription(description);
        ReplaceGenres(genres);
    }

    public void UpdateName(Title newName)
    {
        Name = newName ?? throw new ArgumentNullException(nameof(newName));
    }

    public void UpdateDescription(Description newDescription)
    {
        Description = newDescription ?? throw new ArgumentNullException(nameof(newDescription));
    }

    public void AddGenre(MusicGenre genre)
    {
        if (genre == null) throw new ArgumentNullException(nameof(genre));
        if (_genres.Contains(genre)) 
            throw new InvalidOperationException($"Genre {genre} already exists in concept");
            
        _genres.Add(genre);
    }

    public void AddGenres(IEnumerable<MusicGenre> genres)
    {
        if (genres == null) throw new ArgumentNullException(nameof(genres));
        
        foreach (var genre in genres.Distinct())
        {
            if (!_genres.Contains(genre))
            {
                _genres.Add(genre);
            }
        }
    }

    public void RemoveGenre(MusicGenre genre)
    {
        if (genre == null) throw new ArgumentNullException(nameof(genre));
        if (!_genres.Contains(genre))
            throw new InvalidOperationException($"Genre {genre} not found in concept");
            
        _genres.Remove(genre);
        
        if (_genres.Count == 0)
            throw new InvalidOperationException("Concept must have at least one genre");
    }

    public void ReplaceGenres(IEnumerable<MusicGenre> newGenres)
    {
        if (newGenres == null || !newGenres.Any())
            throw new ArgumentException("At least one genre must be provided", nameof(newGenres));
            
        _genres.Clear();
        _genres.AddRange(newGenres.Distinct());
    }
}