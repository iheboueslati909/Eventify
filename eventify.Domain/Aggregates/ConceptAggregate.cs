using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;

namespace eventify.Domain.Aggregates
{
    public class ConceptAggregate
    {
        private readonly MusicGenreCollection _musicGenres;

        public Guid Id { get; private set; }
        public Guid MemberAggregateId { get; private set; } // Reference to MemberAggregate
        public Name Name { get; private set; }
        public IReadOnlyCollection<MusicGenre> MusicGenres => _musicGenres.Genres; // Expose as IReadOnlyCollection
        private ConceptAggregate() { }

        public ConceptAggregate(Guid memberAggregateId, Name name, MusicGenreCollection musicGenres)
        {
            Id = Guid.NewGuid();
            MemberAggregateId = memberAggregateId; // Reference the MemberAggregate
            Name = name;
            _musicGenres = musicGenres;
        }

        public void UpdateName(Name newName)
        {
            Name = newName;
        }
    }
}