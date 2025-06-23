using System.Text.Json.Serialization;
namespace eventify.Domain.ValueObjects;
using eventify.SharedKernel;

public class Name
{
    public string Value { get; }

    public Name() { } // For deserialization

    [JsonConstructor]
    public Name(string value)
    {
        Value = value;
    }

    public static Result<Name> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Name>("Name cannot be empty.");

        if (value.Length > 100)
            return Result.Failure<Name>("Name must not exceed 100 characters.");

        return Result.Success(new Name(value));
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) =>
        obj is Name name && Value == name.Value;

    public override int GetHashCode() => Value.GetHashCode();
}
