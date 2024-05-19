using W3TL.Core.Domain.Common.UnitOfWork;

namespace Persistance.UnitOfWork;

/// <summary>
/// Represents a unit of work for managing database transactions.
/// </summary>
public class UnitOfWork : IUnitOfWork {
    // private readonly W3TLDbContext _context;
    //
    // public UnitOfWork(W3TLDbContext context) {
    //     _context = context;
    // }

    /// <summary>
    /// Asynchronously saves changes to the underlying database.
    /// </summary>
    /// <remarks>
    /// This method should be called after performing all operations within a unit of work to persist changes.
    /// </remarks>
    public async Task SaveChangesAsync() {
        // await _context.SaveChangesAsync();
    }
}