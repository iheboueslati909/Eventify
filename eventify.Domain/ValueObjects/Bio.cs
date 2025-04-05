namespace eventify.Domain.ValueObjects;

public class Bio
{
    public string Value { get; }

    private Bio() { } // Required for EF Core

    public Bio(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Bio cannot be empty.");

        Value = value;
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) =>
        obj is Bio bio && Value == bio.Value;

    public override int GetHashCode() => Value.GetHashCode();
}