namespace eventify.Domain.ValueObjects;

public class Name
{
    public string Value { get; }

    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Name cannot be empty.");

        Value = value;
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) =>
        obj is Name name && Value == name.Value;

    public override int GetHashCode() => Value.GetHashCode();
}
