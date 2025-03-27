using eventify.Domain.Entities;
using System.Collections.Generic;

namespace eventify.Domain.Aggregates;

public class EventAggregate
{
    public Event Event { get; private set; }
    public List<BookingInvitation> BookingInvitations { get; private set; } = new();
    public List<MemberEvent> MemberEvents { get; private set; } = new();

    private EventAggregate() { } // Required for EF Core

    public EventAggregate(Event eventEntity)
    {
        Event = eventEntity;
    }

    public void AddBookingInvitation(BookingInvitation invitation)
    {
        BookingInvitations.Add(invitation);
    }

    public void AddMemberEvent(MemberEvent memberEvent)
    {
        MemberEvents.Add(memberEvent);
    }
}
