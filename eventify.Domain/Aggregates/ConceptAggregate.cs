using eventify.Domain.Enums;

namespace eventify.Domain.Aggregates
{
    public class ConceptAggregate
    {
        private readonly MusicGenreCollection _musicGenres = new();

        public Guid Id { get; private set; }
        public Guid MemberAggregateId { get; private set; } // Reference to MemberAggregate
        public Name Name { get; private set; }
        public IReadOnlyCollection<MusicGenre> MusicGenres => _musicGenres.AsReadOnly(); // Expose as IReadOnlyCollection
        private ConceptAggregate() { }

        public ConceptAggregate(Guid memberAggregateId, string name, MusicGenreCollection musicGenres)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Concept name cannot be empty.", nameof(name));

            if (musicGenres == null || musicGenres.Count == 0)
                throw new ArgumentException("At least one music genre must be specified.", nameof(musicGenres));

            Id = Guid.NewGuid();
            MemberAggregateId = memberAggregateId; // Reference the MemberAggregate
            Name = name;
            _musicGenres = musicGenres;
        }

        public void UpdateMusicGenres(List<MusicGenre> newGenres)
        {
            if (newGenres == null || newGenres.Count == 0)
                throw new ArgumentException("Music genres list cannot be empty.", nameof(newGenres));

            _musicGenres.Clear();
            _musicGenres.AddRange(newGenres);
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Concept name cannot be empty.", nameof(newName));

            Name = newName;
        }
    }
}