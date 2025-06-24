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

    private readonly List<Guid> _ownerMemberIds = new();
    public IReadOnlyCollection<Guid> OwnerMemberIds => _ownerMemberIds.AsReadOnly();

    private Club() { }

    private Club(Name name, Location address, int capacity, IEnumerable<Guid> ownerMemberIds)
    {
        Id = Guid.NewGuid();
        Name = name;
        Address = address;
        Capacity = capacity;
        if (ownerMemberIds != null)
            _ownerMemberIds.AddRange(ownerMemberIds.Distinct());
    }

    public static Result<Club> Create(Name name, Location address, int capacity, IEnumerable<Guid> ownerMemberIds)
    {
        if (name == null)
            return Result.Failure<Club>("Name cannot be null.");
        if (address == null)
            return Result.Failure<Club>("Address cannot be null.");
        if (capacity <= 0)
            return Result.Failure<Club>("Capacity must be positive.");
        if (ownerMemberIds == null || !ownerMemberIds.Any())
            return Result.Failure<Club>("At least one owner is required.");

        return Result.Success(new Club(name, address, capacity, ownerMemberIds));
    }

    public Result AddOwner(Guid memberId)
    {
        if (_ownerMemberIds.Contains(memberId))
            return Result.Failure("Member is already an owner.");
        _ownerMemberIds.Add(memberId);
        return Result.Success();
    }

    public Result RemoveOwner(Guid memberId)
    {
        if (!_ownerMemberIds.Contains(memberId))
            return Result.Failure("Member is not an owner.");
        _ownerMemberIds.Remove(memberId);
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
