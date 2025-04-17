using eventify.Domain.Enums;
using eventify.SharedKernel;

namespace eventify.Domain.ValueObjects;

public class MusicGenreCollection
{
    private readonly List<MusicGenre> _genres;

    public IReadOnlyCollection<MusicGenre> Genres => _genres.AsReadOnly();

    public MusicGenreCollection(IEnumerable<MusicGenre> genres)
    {
        _genres = genres?.Distinct().ToList() ?? throw new ArgumentNullException(nameof(genres));
        if (!_genres.Any())
            throw new ArgumentException("At least one music genre must be specified.");
    }

    public static MusicGenreCollection FromString(string serialized)
    {
        var genres = serialized
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => int.TryParse(s.Trim(), out var val) && Enum.IsDefined(typeof(MusicGenre), val)
                ? (MusicGenre?)val
                : null)
            .Where(g => g.HasValue)
            .Select(g => g.Value);

        return new MusicGenreCollection(genres);
    }

    public override string ToString() =>
        string.Join(",", _genres.Select(g => ((int)g).ToString()));

    public List<MusicGenre> ToList() => _genres.ToList();

    public static MusicGenreCollection Empty => new([]);

    public override bool Equals(object obj)
    {
        return obj is MusicGenreCollection other &&
               _genres.OrderBy(x => x).SequenceEqual(other._genres.OrderBy(x => x));
    }

    public override int GetHashCode()
    {
        return _genres
            .OrderBy(x => x)
            .Aggregate(0, (hash, genre) => HashCode.Combine(hash, genre.GetHashCode()));
    }

    // Optional helper for adding/removing
    public MusicGenreCollection Add(MusicGenre genre) =>
        new(_genres.Append(genre));

    public MusicGenreCollection Remove(MusicGenre genre) =>
        new(_genres.Where(g => g != genre));
}
