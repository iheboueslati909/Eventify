public interface ICommandDispatcher
{
    Task<TResult> Dispatch<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default);
}
