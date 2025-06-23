using System.Text.Json.Serialization;
namespace eventify.Domain.ValueObjects;
using eventify.SharedKernel;

public class Title
{
    public string Value { get; }

    public Title() { } // For deserialization

    [JsonConstructor]
    public Title(string value)
    {
        Value = value;
    }

    public static Result<Title> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<Title>.Failure("Title cannot be empty.");

        if (value.Length > 100)
            return Result<Title>.Failure("Title must not exceed 100 characters.");

        return Result<Title>.Success(new Title(value));
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) =>
        obj is Title title && Value == title.Value;

    public override int GetHashCode() => Value.GetHashCode();
}