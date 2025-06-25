using eventify.Domain.ValueObjects;
using eventify.SharedKernel;

namespace eventify.Domain.Entities;

public class Club
{
    public Guid Id { get; private set; }
    public Name Name { get; private set; }
    public Location Address { get; private set; }
    public int Capacity { get; private set; }
    public bool IsDeleted { get; private set; }

    private readonly List<Member> _owners = new();
    public IReadOnlyCollection<Member> owners => _owners.AsReadOnly();

    private Club() { }

    private Club(Name name, Location address, int capacity, IEnumerable<Member> owners = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Address = address;
        Capacity = capacity;
        if (owners != null)
            _owners.AddRange(owners.Distinct());
    }

    public static Result<Club> Create(Name name, Location address, int capacity, IEnumerable<Member> owners)
    {
        if (name == null)
            return Result.Failure<Club>("Name cannot be null.");
        if (address == null)
            return Result.Failure<Club>("Address cannot be null.");
        if (capacity <= 0)
            return Result.Failure<Club>("Capacity must be positive.");
        if (owners == null || !owners.Any())
            return Result.Failure<Club>("At least one owner is required.");

        return Result.Success(new Club(name, address, capacity, owners));
    }

    public Result AddOwner(Member member)
    {
        if (member == null)
            return Result.Failure("Member cannot be null.");
        if (_owners.Any(o => o.Id == member.Id))
            return Result.Failure("Member is already an owner.");

        _owners.Add(member);
        return Result.Success();
    }

    public Result RemoveOwner(Member member)
    {
        if (member == null)
            return Result.Failure("Member cannot be null.");
        if (!_owners.Any(o => o.Id == member.Id))
            return Result.Failure("Member is not an owner.");

        _owners.Remove(member);
        return Result.Success();
    }

    public Result ChangeAddress(Location newAddress)
    {
        if (newAddress == null)
            return Result.Failure("New address cannot be null.");
        Address = newAddress;
        return Result.Success();
    }

    public Result ChangeCapacity(int newCapacity)
    {
        if (newCapacity <= 0)
            return Result.Failure("New capacity must be positive.");
        Capacity = newCapacity;
        return Result.Success();
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }
}
