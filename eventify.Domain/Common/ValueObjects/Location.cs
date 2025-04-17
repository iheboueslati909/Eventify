namespace eventify.Domain.ValueObjects;
using eventify.SharedKernel;

public class Location
{
    public string Address { get; }

    private Location(string address)
    {
        Address = address;
    }

    public static Result<Location> Create(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return Result.Failure<Location>("Location address cannot be empty.");

        return Result.Success(new Location(address));
    }

    public override string ToString() => Address;

    public override bool Equals(object? obj) =>
        obj is Location location && Address == location.Address;

    public override int GetHashCode() => Address.GetHashCode();

    //from string
}
