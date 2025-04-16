using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace eventify.Domain.Entities;

//aggregate root
public class Concept
{
    public Guid Id { get; private set; }
    public Guid MemberId { get; private set; }
    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public bool IsDeleted { get; private set; } = false;

    private MusicGenreCollection _genres = MusicGenreCollection.Empty;
    public IReadOnlyCollection<MusicGenre> Genres => _genres.Genres;
    private Concept() { }

    private Concept(Guid memberId, Title title, Description description, MusicGenreCollection  genres)
    {
        Id = Guid.NewGuid();
        MemberId = memberId;
        Title = title;
        Description = description;
        _genres = genres;
    }

    public static Concept Create(Guid memberId, Title title, Description description, MusicGenreCollection genres)
    {
        ArgumentNullException.ThrowIfNull(memberId);
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(description);
        ArgumentNullException.ThrowIfNull(genres);

        return new Concept(memberId, title, description, genres);
    }

    public void UpdateInformation(Title title, Description description, IEnumerable<MusicGenre> genres)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        if (genres == null) throw new ArgumentNullException(nameof(genres));
        _genres = new MusicGenreCollection(genres);
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }
    
}