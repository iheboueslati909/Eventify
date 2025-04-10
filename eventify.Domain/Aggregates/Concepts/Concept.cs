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
    private readonly List<Guid> _eventsIds = new();
    public IReadOnlyCollection<Guid> EventsIds => _eventsIds.AsReadOnly();

    private Concept() { }

    private Concept(Guid memberId, Title title, Description description, IEnumerable<MusicGenre> genres)
    {
        Id = Guid.NewGuid();
        MemberId = memberId;
        Title = title;
        Description = description;
        _genres = new MusicGenreCollection(genres ?? throw new ArgumentNullException(nameof(genres)));
    }

    public static Concept Create(Guid memberId, Title title, Description description, IEnumerable<MusicGenre> genres)
    {
        if (title == null) throw new ArgumentNullException(nameof(title));
        if (description == null) throw new ArgumentNullException(nameof(description));
        if (genres == null) throw new ArgumentNullException(nameof(genres));

        return new Concept(memberId, title, description, genres);
    }

    public void UpdateInformation(Title title, Description description, IEnumerable<MusicGenre> genres)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        if (genres == null) throw new ArgumentNullException(nameof(genres));
        _genres = new MusicGenreCollection(genres);
    }

    public void deleteConcept(List<Event> events)
    {
        if (events.Any(e => e.Status == EventStatus.Published))
            throw new InvalidOperationException("Cannot delete concept with published events.");

        foreach (var eventItem in events)
        {
            if (eventItem.ConceptId == Id)
            {
                eventItem.Cancel();
            }
        }
    }

    public void AddEvent(Guid eventId)
    {
        if (_eventsIds.Contains(eventId))
            throw new InvalidOperationException("Event already exists in this concept.");

        _eventsIds.Add(eventId);
    }

    public void RemoveEvent(Guid eventId)
    {
        if (!_eventsIds.Contains(eventId))
            throw new InvalidOperationException("Event does not exist in this concept.");

        _eventsIds.Remove(eventId);
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }
    
}