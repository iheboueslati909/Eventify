using eventify.Infrastructure.Extensions;
using eventify.SharedKernel;
using MassTransit;
using Messaging.Contracts;

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
        if (!Guid.TryParse(message.IntentId, out var intentGuid)) return;
        if (!Guid.TryParse(message.PaymentId, out var paymentGuid)) return;

        var ticket = await _db.TicketPurchases.FindAsync(intentGuid);
        if (ticket == null) return;

        ticket.MarkAsPaid(paymentGuid);

        await _db.SaveChangesAsync();
    }
}
