using W3TL.Core.Domain.Common.UnitOfWork;

namespace UnitTests.Fakes;

public class FakeUoW : IUnitOfWork {
    public async Task SaveChangesAsync() {
        await Task.CompletedTask;
    }
}