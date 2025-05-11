using eventify.Application.Common;
using Microsoft.Extensions.DependencyInjection;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    //the dispatcher resolve the handler from DI container then call the handle method
    public Task<TResult> Dispatch<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
    {
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        if (handler == null)
        {
            throw new InvalidOperationException($"No handler registered for command type {typeof(TCommand).Name}.");
        }
        return handler.Handle(command, cancellationToken);
    }
}
