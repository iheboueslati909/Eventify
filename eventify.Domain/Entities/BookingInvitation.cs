using eventify.Domain.Enums;
using System;

namespace eventify.Domain.Entities;

public class BookingInvitation
{
    public int Id { get; private set; }
    public int EventId { get; private set; }
    public Event Event { get; private set; }
    public int MemberId { get; private set; }
    public Member Member { get; private set; }
    public BookingStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private BookingInvitation() { } // Required for EF Core

    public BookingInvitation(int eventId, int memberId)
    {
        EventId = eventId;
        MemberId = memberId;
        Status = BookingStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void Accept() => Status = BookingStatus.Accepted;
    public void Reject() => Status = BookingStatus.Rejected;
}
