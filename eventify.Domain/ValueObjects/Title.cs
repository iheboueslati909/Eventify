namespace eventify.Domain.ValueObjects;

public class Title
{
    public string Value { get; }

    public Title(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Title cannot be empty.");

        Value = value;
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) =>
        obj is Title title && Value == title.Value;

    public override int GetHashCode() => Value.GetHashCode();
}
