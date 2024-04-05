using W3TL.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Common.Repository;

public interface IRepository<TAgg, in TId> where TAgg : AggregateRoot<TId> where TId : IdentityBase {
    Task<Result> AddAsync(TAgg aggregate);
    Task<Result> UpdateAsync(TAgg aggregate);
    Task<Result> DeleteAsync(TId id);
    Task<Result<TAgg>> GetByIdAsync(TId id);
    Task<Result<List<TAgg>>> GetAllAsync();
}