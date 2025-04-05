namespace eventify.Domain.ValueObjects;

public class Description
{
    public string Value { get; }

    public Description(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Description cannot be empty.");

        Value = value;
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) =>
        obj is Description description && Value == description.Value;

    public override int GetHashCode() => Value.GetHashCode();
}
