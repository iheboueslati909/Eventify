using EventsManagement.Domain.ValueObjects;
using System.Collections.Generic;

namespace eventify.Domain.Entities;

public class Member
{
    public int Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public List<BookingInvitation> BookingInvitations { get; private set; } = new();
    public List<Event> SavedEvents { get; private set; } = new();

    private Member() { }

    public Member(string firstName, string lastName, Email email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name is required.");
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name is required.");
        if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Password is required.");

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
    }
}
