using eventify.Application.Common;
using eventify.Domain.Entities;
using eventify.SharedKernel;
using eventify.Application.Repositories;
using eventify.Domain.ValueObjects;
using eventify.Domain.Common.Enums; // Add for Currency enum

namespace eventify.Application.Tickets.Commands;

public record CreateTicketCommand(
    Guid EventId,
    Guid CreatorId,
    decimal Price,
    string Name,
    int Quantity, // Added
    Currency Currency // Added
) : ICommand<Result<Guid>>;

public class CreateTicketCommandHandler : ICommandHandler<CreateTicketCommand, Result<Guid>>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IMemberRepository _memberRepository;

    public CreateTicketCommandHandler(
        ITicketRepository ticketRepository,
        IEventRepository eventRepository,
        IMemberRepository memberRepository)
    {
        _ticketRepository = ticketRepository;
        _eventRepository = eventRepository;
        _memberRepository = memberRepository;
    }

    public async Task<Result<Guid>> Handle(CreateTicketCommand command, CancellationToken cancellationToken)
    {
        //TODO test event date
        var @event = await _eventRepository.GetByIdAsync(command.EventId);
        if (@event == null)
            return Result.Failure<Guid>("Event not found.");

        var creator = await _memberRepository.GetByIdAsync(command.CreatorId);
        if (creator == null)
            return Result.Failure<Guid>("Creator (member) not found.");

        var nameResult = Name.Create(command.Name);
        if (nameResult.IsFailure)
            return Result.Failure<Guid>(nameResult.Error);

        var ticketResult = Ticket.Create(
            command.EventId,
            command.CreatorId,
            command.Price,
            nameResult.Value,
            command.Quantity,
            0,
            command.Currency
        );
        if (ticketResult.IsFailure)
            return Result.Failure<Guid>(ticketResult.Error);

        await _ticketRepository.AddAsync(ticketResult.Value);
        await _ticketRepository.SaveChangesAsync();

        return Result.Success(ticketResult.Value.Id);
    }
}
