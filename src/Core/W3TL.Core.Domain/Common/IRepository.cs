using W3TL.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Common;

/// <summary>
/// Represents a generic repository interface for CRUD operations on aggregate roots.
/// </summary>
/// <typeparam name="TAgg">The type of aggregate root.</typeparam>
/// <typeparam name="TId">The type of identity for the aggregate root.</typeparam>
public interface IRepository<TAgg, in TId> where TAgg : AggregateRoot<TId> where TId : IdentityBase {
    /// <summary>
    /// Asynchronously adds a new aggregate root to the repository.
    /// </summary>
    /// <param name="aggregate">The aggregate root to add.</param>
    /// <returns>A task representing the asynchronous operation and containing the result of the operation.</returns>
    Task<Result> AddAsync(TAgg aggregate);

    /// <summary>
    /// Asynchronously updates an existing aggregate root in the repository.
    /// </summary>
    /// <param name="aggregate">The aggregate root to update.</param>
    /// <returns>A task representing the asynchronous operation and containing the result of the operation.</returns>
    Task<Result> UpdateAsync(TAgg aggregate);

    /// <summary>
    /// Asynchronously deletes an aggregate root from the repository based on its identity.
    /// </summary>
    /// <param name="id">The identity of the aggregate root to delete.</param>
    /// <returns>A task representing the asynchronous operation and containing the result of the operation.</returns>
    Task<Result> DeleteAsync(TId id);

    /// <summary>
    /// Asynchronously retrieves an aggregate root from the repository based on its identity.
    /// </summary>
    /// <param name="id">The identity of the aggregate root to retrieve.</param>
    /// <returns>A task representing the asynchronous operation and containing the result of the operation.</returns>
    Task<Result<TAgg>> GetByIdAsync(TId id);

    /// <summary>
    /// Asynchronously retrieves all aggregate roots from the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous operation and containing the result of the operation.</returns>
    Task<Result<List<TAgg>>> GetAllAsync();
}