using eventify.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace eventify.Infrastructure.Persistence.Converters;

public static class MusicGenreCollectionConverter
{
    public static readonly ValueConverter<MusicGenreCollection, string> ToStringConverter =
        new(
            v => v.ToString(),
            v => MusicGenreCollection.FromString(v)
        );
}
