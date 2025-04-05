using EventsManagement.Domain.ValueObjects;
namespace eventify.Domain.Entities;

public class Member
{
    public Guid Id { get; private set; }
    public Name FirstName { get; private set; }
    public Name LastName { get; private set; }
    public Email Email { get; private set; }
    public Password Password { get; private set; }

    private Member() { }

    public Member(string firstName, string lastName, Email email, Password password)
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name is required.");
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name is required.");
        if (password == null) throw new ArgumentException("Password is required.");

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
    }
}
