using System.Collections.Generic;

namespace eventify.Domain.Entities;

public class Club
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string MapsLink { get; private set; }
    public bool IsVerified { get; private set; }
    public List<Event> Events { get; private set; } = new();

    private Club() { } // Required for EF Core

    public Club(string title, string mapsLink)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Club title is required.");

        Title = title;
        MapsLink = mapsLink;
        IsVerified = false;
    }

    public void VerifyClub() => IsVerified = true;
}
