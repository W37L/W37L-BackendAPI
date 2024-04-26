using W3TL.Core.Domain.Common.UnitOfWork;

namespace Persistance.UnitOfWork;

public class UnitOfWork : IUnitOfWork {
    // private readonly W3TLDbContext _context;
    //
    // public UnitOfWork(W3TLDbContext context) {
    //     _context = context;
    // }

    public async Task SaveChangesAsync() {
        // await _context.SaveChangesAsync();
    }
}