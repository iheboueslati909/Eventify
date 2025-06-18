using eventify.Application.Common;
using eventify.SharedKernel;

namespace eventify.Application.Auth.Commands;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : ICommand<Result<Guid>>;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, Result<Guid>>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<Guid>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        return await _identityService.RegisterAsync(
            command.FirstName,
            command.LastName,
            command.Email,
            command.Password);
    }
}
