using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;
using eventify.SharedKernel;
namespace eventify.Domain.Entities;

//aggregate root
public class Member
{
    public Guid Id { get; private set; }
    public Name FirstName { get; private set; }
    public Name LastName { get; private set; }
    public Email Email { get; private set; }

    public bool IsDeleted { get; private set; } = false;

    private readonly List<ArtistProfile> _artistProfiles = new();
    public IReadOnlyCollection<ArtistProfile> ArtistProfiles => _artistProfiles.AsReadOnly();

    // Ticket navigation
    private readonly List<Ticket> _tickets = new();
    public IReadOnlyCollection<Ticket> Tickets => _tickets.AsReadOnly();
    public ICollection<TicketPurchase> TicketPurchases { get; private set; } = new List<TicketPurchase>();

    private Member() { }

    private Member(Name firstName, Name lastName, Email email)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public static Result<Member> Create(Name firstName, Name lastName, Email email)
    {
        return Result.Success(new Member(firstName, lastName, email));
    }

    public Result UpdateInformation(Name firstName, Name lastName, Email email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        return Result.Success();
    }

    public Result SoftDelete()
    {
        if (_artistProfiles.Any(p => !p.IsDeleted))
            return Result.Failure("Cannot delete member with active artist profiles");

        IsDeleted = true;
        return Result.Success();
    }
}
