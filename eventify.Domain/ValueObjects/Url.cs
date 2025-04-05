using System.Text.RegularExpressions;

namespace eventify.Domain.ValueObjects
{
    public class Url
    {
        public string Value { get; private set; }

        private Url() { }

        public Url(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL cannot be empty.");

            if (!Regex.IsMatch(url, @"^(https?|ftp):\/\/[\w\-]+(\.[\w\-]+)+([\w\-.,@?^=%&:/~+#]*[\w\-@?^=%&/~+#])?$"))
                throw new ArgumentException("Invalid URL format.");

            Value = url;
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj) =>
            obj is Url url && Value == url.Value;

        public override int GetHashCode() => Value.GetHashCode();
    }
}
