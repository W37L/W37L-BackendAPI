namespace QueryContracts.Contracts;

/// <summary>
/// Interface representing a query handler.
/// </summary>
/// <typeparam name="TQuery">The type of query to be handled.</typeparam>
/// <typeparam name="TAnswer">The type of the answer returned by the query handler.</typeparam>
public interface IQueryHandler<TQuery, TAnswer> {
    
    /// <summary>
    /// Asynchronously handles a query of type `TQuery`.
    /// </summary>
    /// <param name="query">The query to be handled.</param>
    /// <returns>A task that resolves to the answer of type `TAnswer`.</returns>
    public Task<TAnswer> HandleAsync(TQuery query);
}