using QueryContracts.Contracts;

namespace QueryContracts.QueryDispatching;

public class QueryDispatcher : IQueryDispatcher {
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public Task<TAnswer> DispatchAsync<TAnswer>(IQuery<TAnswer> query) {
        var queryType = query.GetType();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TAnswer));
        dynamic handler = _serviceProvider.GetService(handlerType);
        return handler.HandleAsync((dynamic)query);
    }
}