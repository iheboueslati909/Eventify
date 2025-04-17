using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using eventify.SharedKernel;

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

    private Concept(Guid memberId, Title title, Description description, MusicGenreCollection genres)
    {
        Id = Guid.NewGuid();
        MemberId = memberId;
        Title = title;
        Description = description;
        _genres = genres;
    }

    public static Result<Concept> Create(Guid memberId, Title title, Description description, MusicGenreCollection genres)
    {
        if (memberId == Guid.Empty)
            return Result.Failure<Concept>("Member ID cannot be empty.");
        if (title == null)
            return Result.Failure<Concept>("Title cannot be null.");
        if (description == null)
            return Result.Failure<Concept>("Description cannot be null.");
        if (genres == null)
            return Result.Failure<Concept>("Genres cannot be null.");

        return Result.Success(new Concept(memberId, title, description, genres));
    }

    public Result UpdateInformation(Title title, Description description, IEnumerable<MusicGenre> genres)
    {
        if (title == null)
            return Result.Failure("Title cannot be null.");
        if (description == null)
            return Result.Failure("Description cannot be null.");
        if (genres == null)
            return Result.Failure("Genres cannot be null.");

        Title = title;
        Description = description;
        _genres = new MusicGenreCollection(genres);
        return Result.Success();
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }
    
}