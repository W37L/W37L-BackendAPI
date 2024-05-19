using W3TL.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Common.Repository;

/// <summary>
/// Represents a generic repository interface for aggregate roots in the domain.
/// </summary>
/// <typeparam name="TAgg">The type of the aggregate root.</typeparam>
/// <typeparam name="TId">The type of the identifier for the aggregate root.</typeparam>
public interface IRepository<TAgg, in TId> where TAgg : AggregateRoot<TId> where TId : IdentityBase
{
    /// <summary>
    /// Adds a new aggregate root to the repository.
    /// </summary>
    /// <param name="aggregate">The aggregate root to add.</param>
    /// <returns>A task representing the asynchronous operation and containing the result of the operation.</returns>
    Task<Result> AddAsync(TAgg aggregate);

    /// <summary>
    /// Updates an existing aggregate root in the repository.
    /// </summary>
    /// <param name="aggregate">The aggregate root to update.</param>
    /// <returns>A task representing the asynchronous operation and containing the result of the operation.</returns>
    Task<Result> UpdateAsync(TAgg aggregate);

    /// <summary>
    /// Deletes an aggregate root from the repository by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the aggregate root to delete.</param>
    /// <returns>A task representing the asynchronous operation and containing the result of the operation.</returns>
    Task<Result> DeleteAsync(TId id);

    /// <summary>
    /// Gets an aggregate root from the repository by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the aggregate root to retrieve.</param>
    /// <returns>A task representing the asynchronous operation and containing the result of the operation.</returns>
    Task<Result<TAgg>> GetByIdAsync(TId id);

    /// <summary>
    /// Gets all aggregate roots from the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous operation and containing the result of the operation.</returns>
    Task<Result<List<TAgg>>> GetAllAsync();
}