using eventify.Application.Common;
using eventify.SharedKernel;

namespace eventify.Application.Auth.Commands;

public record LoginCommand(string Email, string Password) : ICommand<Result<string>>;

public class LoginCommandHandler : ICommandHandler<LoginCommand, Result<string>>
{
    private readonly IIdentityService _identityService;

    public LoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<string>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        return await _identityService.LoginAsync(command.Email, command.Password);
    }
}
