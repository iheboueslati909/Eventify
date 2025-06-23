using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using eventify.SharedKernel;

namespace eventify.Domain.ValueObjects;

public class Email
{
    public string Value { get; }

    public Email() { } // For deserialization

    [JsonConstructor]
    public Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Email>("Email cannot be empty.");

        if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            return Result.Failure<Email>("Invalid email format.");

        return Result.Success(new Email(value));
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) =>
        obj is Email email && Value == email.Value;

    public override int GetHashCode() => Value.GetHashCode();
}
