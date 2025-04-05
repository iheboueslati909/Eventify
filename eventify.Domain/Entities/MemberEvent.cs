namespace eventify.Domain.Entities;

public class MemberEvent
{
    public bool IsInterested { get; private set; }
    public bool IsCheckedIn { get; private set; }
    public DateTime? PaymentDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastModified { get; private set; } // Added modification tracking

    // Navigation properties (assuming these exist for EF Core)
    public Guid MemberId { get; private set; }
    public Guid EventId { get; private set; }

    private MemberEvent() { } // Required for EF Core

    public MemberEvent(Guid memberId, Guid eventId)
    {
        if (memberId == Guid.Empty) throw new ArgumentException("Member ID cannot be empty", nameof(memberId));
        if (eventId == Guid.Empty) throw new ArgumentException("Event ID cannot be empty", nameof(eventId));

        MemberId = memberId;
        EventId = eventId;
        IsInterested = true;
        IsCheckedIn = false;
        PaymentDate = null;
        CreatedAt = DateTime.UtcNow;
    }

    public void CheckIn()
    {
        if (IsCheckedIn)
            throw new InvalidOperationException("Member is already checked in to this event");
        
        if (PaymentDate == null)
            throw new InvalidOperationException("Cannot check in without payment");

        IsCheckedIn = true;
        LastModified = DateTime.UtcNow;
    }

    public void RecordPayment()
    {
        if (PaymentDate.HasValue)
            throw new InvalidOperationException("Payment already recorded for this event");

        PaymentDate = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;
    }

    public void ExpressInterest()
    {
        if (IsInterested)
            throw new InvalidOperationException("Member already expressed interest");

        IsInterested = true;
        LastModified = DateTime.UtcNow;
    }

    public void RemoveInterest()
    {
        if (!IsInterested)
            throw new InvalidOperationException("Member already removed interest");

        if (IsCheckedIn)
            throw new InvalidOperationException("Cannot remove interest after checking in");

        IsInterested = false;
        LastModified = DateTime.UtcNow;
    }

    public void CancelCheckIn()
    {
        if (!IsCheckedIn)
            throw new InvalidOperationException("Member is not checked in");

        IsCheckedIn = false;
        LastModified = DateTime.UtcNow;
    }
}