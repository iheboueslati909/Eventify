namespace eventify.Domain.ValueObjects;
using eventify.SharedKernel;

public class Password
{
    public string Hash { get; }

    private Password() { } // Required for EF Core

    private Password(string hash)
    {
        Hash = hash;
    }

    public static Result<Password> Create(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            return Result.Failure<Password>("Password hash cannot be empty.");

        return Result.Success(new Password(hash));
    }

    public override string ToString() => Hash;

    public override bool Equals(object? obj) =>
        obj is Password password && Hash == password.Hash;

    public override int GetHashCode() => Hash.GetHashCode();
}
