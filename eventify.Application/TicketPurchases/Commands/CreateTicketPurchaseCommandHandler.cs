using eventify.Application.Common;
using eventify.Domain.Entities;
using eventify.Domain.Enums;
using eventify.SharedKernel;
using eventify.Application.Common.Interfaces;
using eventify.Application.Repositories;

namespace eventify.Application.Tickets.Commands;

public record CreateTicketPurchaseResult(Guid TicketPurchaseId, string CheckoutUrl, string PaymentId);

public record CreateTicketPurchaseCommand(
    Guid TicketId,
    Guid UserId,
    Guid? PaymentId,
    string PaymentMethod = "stripe" // Default to Stripe, can be extended later
) : ICommand<Result<CreateTicketPurchaseResult>>;

public class CreateTicketPurchaseCommandHandler : ICommandHandler<CreateTicketPurchaseCommand, Result<CreateTicketPurchaseResult>>
{
    private readonly ITicketPurchaseRepository _ticketPurchaseRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IPaymentService _paymentService;

    public CreateTicketPurchaseCommandHandler(
        ITicketPurchaseRepository ticketPurchaseRepository,
        ITicketRepository ticketRepository,
        IMemberRepository memberRepository,
        IPaymentService paymentService)
    {
        _ticketPurchaseRepository = ticketPurchaseRepository;
        _ticketRepository = ticketRepository;
        _memberRepository = memberRepository;
        _paymentService = paymentService;
    }

    public async Task<Result<CreateTicketPurchaseResult>> Handle(CreateTicketPurchaseCommand command, CancellationToken cancellationToken)
    {
        // ✅ Validate ticket and user before transaction
        var ticket = await _ticketRepository.GetByIdWithEventAndCreatorAsync(command.TicketId);
        if (ticket == null)
            return Result.Failure<CreateTicketPurchaseResult>("Ticket not found.");

        var user = await _memberRepository.GetByIdAsync(command.UserId);
        if (user == null)
            return Result.Failure<CreateTicketPurchaseResult>("User not found.");

        if (ticket.Event is null)
            return Result.Failure<CreateTicketPurchaseResult>("Ticket is not linked to an event.");

        if (!ticket.Event.IsPublished || ticket.Event.EndDate < DateTime.UtcNow)
            return Result.Failure<CreateTicketPurchaseResult>("Event is not valid for purchase.");

        // ✅ Reserve logic inside domain
        var reserveResult = ticket.Reserve();
        if (reserveResult.IsFailure)
            return Result.Failure<CreateTicketPurchaseResult>(reserveResult.Error);

        // ✅ Create and save the pending ticket purchase
        var purchaseResult = TicketPurchase.Create(command.TicketId, command.UserId, command.PaymentId);
        if (purchaseResult.IsFailure)
            return Result.Failure<CreateTicketPurchaseResult>(purchaseResult.Error);

        await _ticketPurchaseRepository.AddAsync(purchaseResult.Value);
        await _ticketRepository.UpdateAsync(ticket); // ensure ReservedCount is tracked
        await _ticketPurchaseRepository.SaveChangesAsync();

        var paymentResult = await _paymentService.InitiatePaymentSession(
            command.TicketId,
            command.UserId,
            ticket.Price,
            "usd",
            command.PaymentMethod);

        if (paymentResult.IsFailure)
        {
            purchaseResult.Value.Cancel();
            ticket.Unreserve();
            await _ticketPurchaseRepository.UpdateAsync(purchaseResult.Value);
            await _ticketRepository.UpdateAsync(ticket);
            await _ticketPurchaseRepository.SaveChangesAsync();
            return Result.Failure<CreateTicketPurchaseResult>("Could not initiate payment session: " + paymentResult.Error);
        }

        var paymentSession = paymentResult.Value;
        var result = new CreateTicketPurchaseResult(
            purchaseResult.Value.Id,
            paymentSession.CheckoutUrl,
            paymentSession.PaymentId
        );

        return Result.Success(result);
    }

}
