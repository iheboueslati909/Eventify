namespace eventify.Domain.Entities;

public class MemberEvent
{
    public bool IsInterested { get; private set; } // Tracks interest
    public bool IsCheckedIn { get; private set; } // Tracks check-in status
    public DateTime? PaymentDate { get; private set; } // Tracks payment date
    public DateTime CreatedAt { get; private set; } // Tracks when the relationship was created

    private MemberEvent() { } // Required for EF Core

    public MemberEvent()
    {
        IsInterested = true;
        IsCheckedIn = false;
        PaymentDate = null;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsCheckedIn()
    {
        if (IsCheckedIn)
            throw new InvalidOperationException("Member is already checked in.");

        IsCheckedIn = true;
    }

    public void RecordPayment(DateTime paymentDate)
    {
        if (PaymentDate != null)
            throw new InvalidOperationException("Payment is already recorded.");

        PaymentDate = paymentDate;
    }

    public void RemoveInterest()
    {
        IsInterested = false;
    }
}
