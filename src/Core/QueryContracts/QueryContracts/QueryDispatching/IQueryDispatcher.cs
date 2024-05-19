using QueryContracts.Contracts;

namespace QueryContracts.QueryDispatching;

/// <summary>
///     The interface for query dispatching.
/// </summary>
public interface IQueryDispatcher {
    /// <summary>
    ///     Asynchronously dispatches a query object and returns the result.
    /// </summary>
    /// <typeparam name="TAnswer">The type of the query answer.</typeparam>
    /// <param name="query">The query object to dispatch.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<TAnswer> DispatchAsync<TAnswer>(IQuery<TAnswer> query);
}