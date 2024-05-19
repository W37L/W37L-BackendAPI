using QueryContracts.Contracts;

namespace QueryContracts.QueryDispatching;

/// <summary>
///     Represents a class for dispatching queries.
/// </summary>
public class QueryDispatcher : IQueryDispatcher {
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }
    /// <summary>
    ///     Asynchronously dispatches a query to its corresponding handler and retrieves the answer.
    /// </summary>
    /// <typeparam name="TAnswer">The type of the answer expected from the query.</typeparam>
    /// <param name="query">The query to be dispatched.</param>
    /// <returns>A task that resolves to the answer returned by the query handler.</returns>
    public Task<TAnswer> DispatchAsync<TAnswer>(IQuery<TAnswer> query) {
        var queryType = query.GetType();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TAnswer));
        dynamic handler = _serviceProvider.GetService(handlerType);
        return handler.HandleAsync((dynamic)query);
    }
}