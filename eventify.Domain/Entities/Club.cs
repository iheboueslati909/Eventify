namespace eventify.Domain.Entities;
using eventify.Domain.ValueObjects;

public class Club
{
    public Guid Id { get; private set; }
    public Title Title { get; private set; }
    public Location? MapsLink { get; private set; }
    public bool IsVerified { get; private set; }

    private Club() { } // Required for EF Core

    public Club(string title)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Club title is required.");

        Title = title;
        IsVerified = false;
    }

    public void VerifyClub() => IsVerified = true;

    public void UpdateMapsLink(Location? mapsLink)
    {
        MapsLink = mapsLink;
    }
}
