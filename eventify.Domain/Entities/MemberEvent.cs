using System;

namespace eventify.Domain.Entities;

public class MemberEvent
{
    public int MemberId { get; private set; }
    public Member Member { get; private set; }

    public int EventId { get; private set; }
    public Event Event { get; private set; }

    public bool Attended { get; private set; }  // Track attendance
    public DateTime SavedAt { get; private set; } // Track when event was saved

    private MemberEvent() { } // Required for EF Core

    public MemberEvent(int memberId, int eventId)
    {
        MemberId = memberId;
        EventId = eventId;
        SavedAt = DateTime.UtcNow;
        Attended = false;
    }

    public void MarkAsAttended() => Attended = true;
}
