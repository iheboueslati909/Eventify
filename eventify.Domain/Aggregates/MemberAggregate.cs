using eventify.Domain.Entities;
using System.Collections.Generic;

namespace eventify.Domain.Aggregates;

public class MemberAggregate
{
    public Member Member { get; private set; }
    public List<MemberEvent> MemberEvents { get; private set; } = new();
    public List<BookingInvitation> BookingInvitations { get; private set; } = new();

    private MemberAggregate() { } // Required for EF Core

    public MemberAggregate(Member member)
    {
        Member = member;
    }

    public void AddMemberEvent(MemberEvent memberEvent)
    {
        MemberEvents.Add(memberEvent);
    }

    public void AddBookingInvitation(BookingInvitation invitation)
    {
        BookingInvitations.Add(invitation);
    }
}
