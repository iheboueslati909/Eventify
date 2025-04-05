using eventify.Domain.ValueObjects;

namespace eventify.Domain.Entities;

public class Member
{
    public Guid Id { get; private set; }
    public Name FirstName { get; private set; }
    public Name LastName { get; private set; }
    public Email Email { get; private set; }
    public Password Password { get; private set; }

    private Member() { } // For EF Core

    public Member(Name firstName, Name lastName, Email email, Password password)
    {
        UpdateFirstName(firstName);
        UpdateLastName(lastName);
        UpdateEmail(email);
        UpdatePassword(password);
    }

    public void UpdateFirstName(Name newFirstName)
    {
        FirstName = newFirstName ?? throw new ArgumentNullException(nameof(newFirstName));
    }

    public void UpdateLastName(Name newLastName)
    {
        LastName = newLastName ?? throw new ArgumentNullException(nameof(newLastName));
    }

    public void UpdateEmail(Email newEmail)
    {
        Email = newEmail ?? throw new ArgumentNullException(nameof(newEmail));
    }

    public void UpdatePassword(Password newPassword)
    {
        Password = newPassword ?? throw new ArgumentNullException(nameof(newPassword));
    }

    // Alternative password change method with validation
    public void ChangePassword(Password currentPassword, Password newPassword)
    {
        if (!Password.Equals(currentPassword))
            throw new InvalidOperationException("Current password is incorrect");
        
        UpdatePassword(newPassword);
    }
}