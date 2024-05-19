namespace W3TL.Core.Domain.Common.UnitOfWork;

/// <summary>
/// Represents a unit of work interface for managing changes and committing them to the underlying data store.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Asynchronously saves changes made within the unit of work to the underlying data store.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SaveChangesAsync();
}