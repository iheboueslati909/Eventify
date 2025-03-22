using System;

namespace eventify.Domain.Entities;

public class NewsFeedItem
{
    public int Id { get; private set; }
    public int MemberId { get; private set; }
    public Member Member { get; private set; }
    public string Type { get; private set; } // "Event", "Performance", "ArtistBooking"
    public int RelatedEntityId { get; private set; }
    public string Message { get; private set; }
    public DateTime Timestamp { get; private set; }

    private NewsFeedItem() { } // Required for EF Core

    public NewsFeedItem(int memberId, string type, int relatedEntityId, string message)
    {
        MemberId = memberId;
        Type = type;
        RelatedEntityId = relatedEntityId;
        Message = message;
        Timestamp = DateTime.UtcNow;
    }
}
