using System;
using System.Text.RegularExpressions;

namespace EventsManagement.Domain.ValueObjects;

public class Email
{
    public string Value { get; private set; }

    private Email() { }

    public Email(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.");

        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Invalid email format.");

        Value = email;
    }

    public override string ToString() => Value;
}
