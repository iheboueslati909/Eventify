using eventify.Domain.Enums;
using eventify.SharedKernel;
using eventify.Domain.ValueObjects;
using eventify.Domain.Common.Enums;

namespace eventify.Domain.Entities;

public class Ticket
{
    public Guid Id { get; private set; }
    public Guid EventId { get; private set; }
    public Guid CreatorId { get; private set; }
    public decimal Price { get; private set; }
    public Name Name { get; private set; }
    public int Quantity { get; private set; }
    public int ReservedCount { get; private set; }
    //currency
    public Currency Currency { get; private set; } = Currency.USD; // Default to USD, can be extended later
    public Event Event { get; private set; }
    public Member Creator { get; private set; }

    private Ticket() { }

    private Ticket(Guid eventId, Guid creatorId, decimal price, Name name, int quantity = 0, int reservedCount = 0, Currency currency = Currency.USD)
    {
        if (quantity < 0)
            throw new ArgumentException("Quantity cannot be negative.", nameof(quantity));
        if (reservedCount < 0)
            throw new ArgumentException("Reserved count cannot be negative.", nameof(reservedCount));

        Quantity = quantity;
        ReservedCount = reservedCount;
        Id = Guid.NewGuid();
        EventId = eventId;
        CreatorId = creatorId;
        Price = price;
        Name = name;
        Currency = currency;
    }

    public static Result<Ticket> Create(Guid eventId, Guid creatorId, decimal price, Name name, int quantity = 0, int reservedCount = 0, Currency currency = Currency.USD)
    {
        if (eventId == Guid.Empty)
            return Result.Failure<Ticket>("Event ID cannot be empty.");
        if (creatorId == Guid.Empty)
            return Result.Failure<Ticket>("Member ID cannot be empty.");
        if (price < 0)
            return Result.Failure<Ticket>("Price cannot be negative.");
        if (name == null)
            return Result.Failure<Ticket>("Name cannot be null.");

        return Result.Success(new Ticket(eventId, creatorId, price, name, quantity, reservedCount, currency));
    }

    public bool CanReserve()
    {
        return Quantity - ReservedCount > 0;
    }

    public Result Reserve()
    {
        if (!CanReserve())
            return Result.Failure("No more tickets available.");

        ReservedCount++;
        return Result.Success();
    }

    public Result Unreserve()
    {
        if (ReservedCount <= 0)
            return Result.Failure("No tickets reserved to unreserve.");

        ReservedCount--;
        return Result.Success();
    }

}
