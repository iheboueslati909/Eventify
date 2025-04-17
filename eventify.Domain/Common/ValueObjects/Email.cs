using System.Text.RegularExpressions;
using eventify.Domain.Common;
using eventify.SharedKernel;

namespace eventify.Domain.ValueObjects;

public class Email
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    private Email() { } // Required for EF Core

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
