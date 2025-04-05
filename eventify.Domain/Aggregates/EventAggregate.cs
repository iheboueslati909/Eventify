using eventify.Domain.Enums;
using eventify.Domain.Entities;

namespace eventify.Domain.Aggregates
{
    public class EventAggregate
    {
        public Guid Id { get; private set; }
        public Title Title { get; private set; }
        public Description Description { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public Location Location { get; private set; }
        public EventType EventType { get; private set; }
        public EventStatus Status { get; private set; }
        
        private EventAggregate() { } // Required for EF Core

        public EventAggregate(Title title, Description description, DateTime startDate, DateTime endDate, Location location, EventType eventType)
        {
            if (title == null)
                throw new ArgumentException("Event title cannot be empty.", nameof(title));

            if (startDate >= endDate)
                throw new ArgumentException("Start date must be before end date.", nameof(startDate));

            if (location == null)
                throw new ArgumentException("Location cannot be empty.", nameof(location));

            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Location = location;
            EventType = eventType;
        }

        public void UpdateDetails(Title title, Description description, DateTime startDate, DateTime endDate, Location location, EventType eventType)
        {
            if (title == null)
                throw new ArgumentException("Event title cannot be empty.");

            if (startDate >= endDate)
                throw new ArgumentException("Start date must be before end date.");

            if (location == null)
                throw new ArgumentException("Location cannot be empty.");

            Title = title;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Location = location;
            EventType = eventType; // ✅ Ensure updates include event type
        }

        public void Cancel()
            {
                if (StartDate <= DateTime.UtcNow)
                    throw new DomainException("Cannot cancel an ongoing or past event");
                Status = EventStatus.Cancelled;
                AddDomainEvent(new EventCancelledEvent(Id));
            }
    }
}
