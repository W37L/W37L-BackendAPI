namespace QueryContracts.Contracts;

public interface IQueryHandler<TQuery, TAnswer> {
    public Task<TAnswer> HandleAsync(TQuery query);
}