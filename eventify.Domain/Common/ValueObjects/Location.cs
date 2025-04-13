namespace eventify.Domain.ValueObjects;

public class Location
{
    public string Address { get; }

    public Location(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Location address cannot be empty.");

        Address = address;
    }

    public override string ToString() => Address;

    public override bool Equals(object? obj) =>
        obj is Location location && Address == location.Address;

    public override int GetHashCode() => Address.GetHashCode();

    //from string
}
