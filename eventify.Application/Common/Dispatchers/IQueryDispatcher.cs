
public interface IQueryDispatcher
{
    //takes TQuery and TResult as generic parameters
    //Where TQuery is a class that implements IQuery<TResult>
    Task<TResult> Dispatch<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
    where TQuery : IQuery<TResult>;
}