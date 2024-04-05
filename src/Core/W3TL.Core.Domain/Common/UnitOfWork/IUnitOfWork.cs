namespace W3TL.Core.Domain.Common.UnitOfWork;

public interface IUnitOfWork {
    Task SaveChangesAsync();
}