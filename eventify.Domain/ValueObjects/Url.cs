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

            if (!Regex.IsMatch(url, @"^(https?|ftp):\/\/[^\s/$.?#].[^\s]*$"))
                throw new ArgumentException("Invalid URL format.");

            Value = url;
        }

        public override string ToString() => Value;
    }
}
