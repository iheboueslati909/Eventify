using eventify.Domain.Enums;
using eventify.SharedKernel;

namespace eventify.Domain.ValueObjects;

public class MusicGenreCollection
{
    private readonly List<MusicGenre> _genres;

    public IReadOnlyCollection<MusicGenre> Genres => _genres.AsReadOnly();

    private MusicGenreCollection(IEnumerable<MusicGenre> genres)
    {
        _genres = genres.Distinct().ToList();
    }

    public static Result<MusicGenreCollection> Create(IEnumerable<MusicGenre> genres)
    {
        if (genres == null)
            return Result.Failure<MusicGenreCollection>("Genres collection cannot be null.");

        var genresList = genres.Distinct().ToList();
        
        if (!genresList.Any())
            return Result.Failure<MusicGenreCollection>("At least one music genre must be specified.");

        return Result.Success(new MusicGenreCollection(genresList));
    }

    public static Result<MusicGenreCollection> FromString(string serialized)
    {
        if (string.IsNullOrWhiteSpace(serialized))
            return Result.Failure<MusicGenreCollection>("Serialized genres string cannot be empty.");

        var genres = serialized
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => int.TryParse(s.Trim(), out var val) && Enum.IsDefined(typeof(MusicGenre), val)
                ? (MusicGenre?)val
                : null)
            .Where(g => g.HasValue)
            .Select(g => g.Value);

        return Create(genres);
    }

    public override string ToString() =>
        string.Join(",", _genres.Select(g => ((int)g).ToString()));

    public List<MusicGenre> ToList() => _genres.ToList();

    public static Result<MusicGenreCollection> Empty => Create(new List<MusicGenre>());

    public Result<MusicGenreCollection> Add(MusicGenre genre) =>
        Create(_genres.Append(genre));

    public Result<MusicGenreCollection> Remove(MusicGenre genre) =>
        Create(_genres.Where(g => g != genre));

    public override bool Equals(object? obj) =>
        obj is MusicGenreCollection other &&
        _genres.OrderBy(x => x).SequenceEqual(other._genres.OrderBy(x => x));

    public override int GetHashCode() =>
        _genres.OrderBy(x => x)
            .Aggregate(0, (hash, genre) => HashCode.Combine(hash, genre.GetHashCode()));
}
