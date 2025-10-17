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
    private readonly ITicketRepository _ticketRepository;

    public FullFillTicketPurchaseCommandHandler(ITicketPurchaseRepository ticketPurchaseRepository,
    ITicketRepository ticketRepository)
    {
        _ticketPurchaseRepository = ticketPurchaseRepository;
        _ticketRepository = ticketRepository;
    }

    public async Task<Result> Handle(FullFillTicketPurchaseCommand command, CancellationToken cancellationToken)
        //TODO test event date
    {
        var purchase = await _ticketPurchaseRepository.GetByIdAsync(command.TicketPurchaseId);
        if (purchase == null)
            return Result.Failure("Ticket purchase not found.");

        var ticket = await _ticketRepository.GetByIdAsync(purchase.TicketId);
        if (ticket == null)
            return Result.Failure("Ticket not found");

        var ticketResult = ticket.Reserve();
        if (ticketResult.IsFailure)
            return Result.Failure("could not reserve the ticket/s");

        var result = purchase.MarkAsPaid(command.PaymentId);
        if (result.IsFailure)
            return result;

        await _ticketRepository.SaveChangesAsync();
        await _ticketPurchaseRepository.SaveChangesAsync();
        return Result.Success();
    }
}
