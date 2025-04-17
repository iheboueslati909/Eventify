using eventify.Domain.Common;
using eventify.SharedKernel;

namespace eventify.Domain.ValueObjects;

public class Bio
{
    public string Value { get; }

    private Bio() { } // Required for EF Core

    private Bio(string value)
    {
        Value = value;
    }

    public static Result<Bio> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Bio>("Bio cannot be empty");

        if (value.Length > 5000)
            return Result.Failure<Bio>("Bio cannot exceed 5000 characters");

        return Result.Success(new Bio(value));
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) =>
        obj is Bio bio && Value == bio.Value;

    public override int GetHashCode() => Value.GetHashCode();
}