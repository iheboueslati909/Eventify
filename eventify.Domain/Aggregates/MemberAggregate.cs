using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;

namespace eventify.Domain.Aggregates
{
    public class MemberAggregate
    {
        private readonly List<Guid> _artistProfileIds = new(); // Encapsulated ArtistProfile IDs
        private readonly List<Guid> _followedConceptIds = new(); // Encapsulated FollowedConcept IDs

        public Guid Id { get; private set; }
        public Name FirstName { get; private set; }
        public Name LastName { get; private set; }
        public Email Email { get; private set; }
        public IReadOnlyCollection<Guid> ArtistProfileIds => _artistProfileIds.AsReadOnly();
        public IReadOnlyCollection<Guid> FollowedConceptIds => _followedConceptIds.AsReadOnly();

        private MemberAggregate() { } // Required for EF Core

        public MemberAggregate(Name firstName, Name lastName, Email email)
        {
            Id = Guid.NewGuid();
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName), "First name cannot be null.");
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName), "Last name cannot be null.");
            Email = email ?? throw new ArgumentNullException(nameof(email), "Email cannot be null.");
        }

        public void AddArtistProfile(Guid artistProfileId)
        {
            if (_artistProfileIds.Contains(artistProfileId))
                throw new InvalidOperationException("Artist profile already exists for this member.");

            _artistProfileIds.Add(artistProfileId);
        }

        public void RemoveArtistProfile(Guid artistProfileId)
        {
            if (!_artistProfileIds.Contains(artistProfileId))
                throw new InvalidOperationException("Artist profile not found for this member.");

            _artistProfileIds.Remove(artistProfileId);
        }

        public void FollowConcept(Guid conceptId)
        {
            if (_followedConceptIds.Contains(conceptId))
                throw new InvalidOperationException("Concept already followed.");

            _followedConceptIds.Add(conceptId);
        }

        public void UnfollowConcept(Guid conceptId)
        {
            if (!_followedConceptIds.Contains(conceptId))
                throw new InvalidOperationException("Concept not followed.");

            _followedConceptIds.Remove(conceptId);
        }
    }
}
