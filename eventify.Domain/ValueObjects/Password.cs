namespace eventify.Domain.ValueObjects;

public class Password
{
    public string Hash { get; }

    public Password(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("Password hash cannot be empty.");

        Hash = hash;
    }

    public override string ToString() => Hash;

    public override bool Equals(object? obj) =>
        obj is Password password && Hash == password.Hash;

    public override int GetHashCode() => Hash.GetHashCode();
}
