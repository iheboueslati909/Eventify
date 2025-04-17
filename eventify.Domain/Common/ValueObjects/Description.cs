namespace eventify.Domain.ValueObjects;
using eventify.SharedKernel;

public class Description
{
    public string Value { get; }

    private Description() { } // Required for EF Core

    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<Description>.Failure("Description cannot be empty.");

        if (value.Length > 500)
            return Result<Description>.Failure("Description must not exceed 500 characters.");

        return Result<Description>.Success(new Description(value));
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) =>
        obj is Description description && Value == description.Value;

    public override int GetHashCode() => Value.GetHashCode();
}