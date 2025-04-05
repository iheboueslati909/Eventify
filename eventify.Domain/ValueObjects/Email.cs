using System.Text.RegularExpressions;

namespace eventify.Domain.ValueObjects;

public class Email
{
    public string Value { get; }

    private Email() { } // Required for EF Core

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty.");

        if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Invalid email format.");

        Value = value;
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) =>
        obj is Email email && Value == email.Value;

    public override int GetHashCode() => Value.GetHashCode();
}
