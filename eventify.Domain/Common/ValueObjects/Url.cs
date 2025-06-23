using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using eventify.SharedKernel;

namespace eventify.Domain.ValueObjects
{
    public class Url
{
    public string Value { get; private set; }

    public Url() { } // For deserialization

    [JsonConstructor]
    public Url(string value)
    {
        Value = value;
    }

    public static Result<Url> Create(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return Result.Failure<Url>("URL cannot be empty.");

        if (!Regex.IsMatch(url, @"^(https?|ftp):\/\/[\w\-]+(\.[\w\-]+)+([\w\-.,@?^=%&:/~+#]*[\w\-@?^=%&/~+#])?$"))
            return Result.Failure<Url>("Invalid URL format.");

        return Result.Success(new Url(url));
    }

    public static Url? FromDatabase(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return null;

        return new Url(url); // assumes private constructor bypasses validation for trusted DB data
    }
    public override string ToString() => Value;

    public override bool Equals(object? obj) => obj is Url url && Value == url.Value;

    public override int GetHashCode() => Value.GetHashCode();
}

}
