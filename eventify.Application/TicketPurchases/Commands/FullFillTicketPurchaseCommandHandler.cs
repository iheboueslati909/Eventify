using eventify.Application.Common;
using eventify.Domain.Entities;
using eventify.SharedKernel;
using eventify.Application.Common.Interfaces;
using eventify.Application.Repositories;

namespace eventify.Application.Tickets.Commands;

public record FullFillTicketPurchaseCommand(
    Guid TicketPurchaseId,
    Guid? PaymentId = null
) : ICommand<Result>;

public class FullFillTicketPurchaseCommandHandler : ICommandHandler<FullFillTicketPurchaseCommand, Result>
{
    private readonly ITicketPurchaseRepository _ticketPurchaseRepository;

    public FullFillTicketPurchaseCommandHandler(ITicketPurchaseRepository ticketPurchaseRepository)
    {
        _ticketPurchaseRepository = ticketPurchaseRepository;
    }

    public async Task<Result> Handle(FullFillTicketPurchaseCommand command, CancellationToken cancellationToken)
        //TODO test event date
    {
        var purchase = await _ticketPurchaseRepository.GetByIdAsync(command.TicketPurchaseId);
        if (purchase == null)
            return Result.Failure("Ticket purchase not found.");

        var result = purchase.MarkAsPaid(command.PaymentId);
        if (result.IsFailure)
            return result;

        await _ticketPurchaseRepository.SaveChangesAsync();
        return Result.Success();
    }
}
