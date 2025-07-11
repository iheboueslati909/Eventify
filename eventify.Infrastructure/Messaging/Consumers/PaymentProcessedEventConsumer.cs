using eventify.Infrastructure.Extensions;
using eventify.SharedKernel;
using MassTransit;

namespace eventify.Infrastructure.Messaging.Consumers;

public class PaymentProcessedEventConsumer : IConsumer<PaymentProcessedEvent>
{
    private readonly EventsDbContext _db;

    public PaymentProcessedEventConsumer(EventsDbContext db)
    {
        _db = db;
    }

    public async Task Consume(ConsumeContext<PaymentProcessedEvent> context)
    {
        var message = context.Message;
        Console.WriteLine($"*************PaymentProcessedEventConsumer: {message.PaymentId} - {message.IntentId}");

        if (message.Status != "Succeeded") return;

        var ticket = await _db.TicketPurchases.FindAsync(message.IntentId);
        if (ticket == null) return;

        ticket.MarkAsPaid(message.PaymentId);

        var purchaseResult = eventify.Domain.Entities.TicketPurchase.Create(
            ticket.Id,
            new Guid(message.UserId),
            message.PaymentId
        );
        if (purchaseResult.IsFailure)
        {
            return;
        }
        await _db.SaveChangesAsync();
    }
}
