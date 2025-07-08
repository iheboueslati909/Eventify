using eventify.Domain.Enums;
using eventify.SharedKernel;

namespace eventify.Domain.Entities;

public class TicketPurchase
{
    public Guid Id { get; private set; }
    public Guid TicketId { get; private set; }
    public Guid UserId { get; private set; }
    public Guid? PaymentId { get; private set; }
    public TicketPurchaseStatus Status { get; private set; }
    public DateTime PurchasedAt { get; private set; }

    // Navigation properties
    public Ticket Ticket { get; private set; }
    public Member User { get; private set; }

    private TicketPurchase() { } // For EF Core

    private TicketPurchase(Guid ticketId, Guid userId, Guid? paymentId, TicketPurchaseStatus status)
    {
        Id = Guid.NewGuid();
        TicketId = ticketId;
        UserId = userId;
        PaymentId = paymentId;
        Status = status;
        PurchasedAt = DateTime.UtcNow;
    }

    public static Result<TicketPurchase> Create(Guid ticketId, Guid userId, Guid? paymentId = null)
    {
        if (ticketId == Guid.Empty)
            return Result.Failure<TicketPurchase>("Ticket ID cannot be empty.");
        if (userId == Guid.Empty)
            return Result.Failure<TicketPurchase>("User ID cannot be empty.");

        return Result.Success(new TicketPurchase(ticketId, userId, paymentId, TicketPurchaseStatus.PendingPayment));
    }

    public Result MarkAsPaid(Guid? paymentId = null)
    {
        if (Status != TicketPurchaseStatus.PendingPayment)
            return Result.Failure("Only pending purchases can be marked as paid.");
        Status = TicketPurchaseStatus.Paid;
        if (paymentId != null)
            PaymentId = paymentId;
        return Result.Success();
    }

    public Result Cancel()
    {
        if (Status == TicketPurchaseStatus.Cancelled)
            return Result.Failure("Purchase is already cancelled.");
        Status = TicketPurchaseStatus.Cancelled;
        return Result.Success();
    }

    public Result Refund()
    {
        if (Status != TicketPurchaseStatus.Paid)
            return Result.Failure("Only paid purchases can be refunded.");
        Status = TicketPurchaseStatus.Refunded;
        return Result.Success();
    }
}
