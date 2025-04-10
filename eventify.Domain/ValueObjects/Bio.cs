namespace eventify.Domain.ValueObjects;

public class Bio
{
    public string Value { get; }

    private Bio() { } // Required for EF Core

    public Bio(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Bio cannot be empty", nameof(value));
        if (value.Length > 5000)
            throw new ArgumentException("Bio cannot exceed 500 characters", nameof(value));
        Value = value;
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) =>
        obj is Bio bio && Value == bio.Value;

    public override int GetHashCode() => Value.GetHashCode();
}