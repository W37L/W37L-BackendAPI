using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.Post.Values;

namespace UnitTests.Fakes;

public class InMemContentRepoStub : IContentRepository {
    // set up the in-memory database
    public readonly List<Content> _contents = new();

    public Task<Result> AddAsync(Content aggregate) {
        _contents.Add(aggregate);
        return Task.FromResult(Result.Success());
    }

    public Task<Result> UpdateAsync(Content aggregate) {
        // find the content in the list
        var existingContent = _contents.FirstOrDefault(e => e.Id == aggregate.Id);
        if (existingContent == null) return Task.FromResult(Result.Fail(Error.ContentNotFound));

        // update the content
        existingContent = aggregate;
        return Task.FromResult(Result.Success());
    }

    public Task<Result> DeleteAsync(ContentIDBase id) {
        throw new NotImplementedException();
    }

    public Task<Result<Content>> GetByIdAsync(ContentIDBase id) {
        // find the content in the list
        var existingContent = _contents.FirstOrDefault(e => e.Id == id);
        if (existingContent == null) return Task.FromResult(Result<Content>.Fail(Error.ContentNotFound));

        return Task.FromResult(Result<Content>.Success(existingContent));
    }

    public Task<Result<List<Content>>> GetAllAsync() {
        return Task.FromResult(Result<List<Content>>.Success(_contents));
    }

    public Task<Result> DeleteAsync(PostId id) {
        // find the content in the list
        var existingContent = _contents.FirstOrDefault(e => e.Id == id);
        if (existingContent == null) return Task.FromResult(Result.Fail(Error.ContentNotFound));

        // delete the content
        _contents.Remove(existingContent);
        return Task.FromResult(Result.Success());
    }

    public Task<Result<Content>> GetByIdAsync(PostId id) {
        // find the content in the list
        var existingContent = _contents.FirstOrDefault(e => e.Id == id);
        if (existingContent == null) return Task.FromResult(Result<Content>.Fail(Error.ContentNotFound));

        return Task.FromResult(Result<Content>.Success(existingContent));
    }
}