namespace eventify.Domain.Entities;
using eventify.Domain.ValueObjects;

public class Club
{
    public Guid Id { get; private set; }
    public Title Name { get; private set; }  // Changed from Title to Name for clarity
    public Location? Location { get; private set; }  // Renamed from MapsLink to Location
    public bool IsVerified { get; private set; }

    private Club() { } // Required for EF Core

    public Club(Title name)
    {
        UpdateName(name);
        IsVerified = false;
    }

    public void Verify()
    {
        if (IsVerified)
            throw new InvalidOperationException("Club is already verified");
            
        IsVerified = true;
    }

    public void Unverify()
    {
        if (!IsVerified)
            throw new InvalidOperationException("Club is not verified");

        IsVerified = false;
    }

    public void UpdateName(Title newName)
    {
        Name = newName ?? throw new ArgumentNullException(nameof(newName));
    }

    public void UpdateLocation(Location? newLocation)
    {
        Location = newLocation;  // Null is allowed (Location is nullable)
    }

}