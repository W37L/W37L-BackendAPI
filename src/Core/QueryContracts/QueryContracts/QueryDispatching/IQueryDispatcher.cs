using QueryContracts.Contracts;

namespace QueryContracts.QueryDispatching;

public interface IQueryDispatcher {
    Task<TAnswer> DispatchAsync<TAnswer>(IQuery<TAnswer> query);
}