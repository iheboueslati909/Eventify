using Microsoft.Extensions.DependencyInjection;
namespace eventify.Application.Common;

public class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    //the dispatcher resolve the handler from DI container then call the handle method

    public Task<TResult> Dispatch<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResult>
        {
            var handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
            return handler.Handle(query, cancellationToken);
        }
}
