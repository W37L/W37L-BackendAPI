using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Common.Values;

namespace UnitTests.Fakes;

public class InMemContentRepoStub : IContentRepository {
    private readonly List<Content> _contents = new();

    public Task<Result> AddAsync(Content aggregate) {
        _contents.Add(aggregate);
        return Task.FromResult(Result.Success());
    }

    public Task<Result> UpdateAsync(Content aggregate) {
        var existingContent = _contents.FirstOrDefault(e => e.Id == aggregate.Id);
        if (existingContent == null) return Task.FromResult(Result.Fail(Error.ContentNotFound));

        // Replace existing content with the updated version
        var index = _contents.IndexOf(existingContent);
        _contents[index] = aggregate;
        return Task.FromResult(Result.Success());
    }

    public Task<Result> DeleteAsync(ContentIDBase id) {
        var contentToRemove = _contents.FirstOrDefault(e => e.Id == id);
        if (contentToRemove == null) return Task.FromResult(Result.Fail(Error.ContentNotFound));

        _contents.Remove(contentToRemove);
        return Task.FromResult(Result.Success());
    }

    public Task<Result<Content>> GetByIdAsync(ContentIDBase id) {
        var existingContent = _contents.FirstOrDefault(e => e.Id == id);
        return Task.FromResult(existingContent != null
            ? Result<Content>.Success(existingContent)
            : Result<Content>.Fail(Error.ContentNotFound));
    }

    public Task<Result<List<Content>>> GetAllAsync() {
        return Task.FromResult(Result<List<Content>>.Success(_contents));
    }

    public Task<Result<UserID>> GetAuthorIdAsync(ContentIDBase id) {
        var content = _contents.FirstOrDefault(e => e.Id == id);
        if (content == null) return Task.FromResult(Result<UserID>.Fail(Error.ContentNotFound));

        return Task.FromResult(Result<UserID>.Success(content.Creator.Id));
    }

    public Task<Result<Content>> GetByFullIdAsync(ContentIDBase id, User user) {
        var content = _contents.FirstOrDefault(e => e.Id == id && e.Creator.Id == user.Id);
        return Task.FromResult(content != null
            ? Result<Content>.Success(content)
            : Result<Content>.Fail(Error.ContentNotFound));
    }

    public Task<Result<List<Content>>> GetPostsByUserIdAsync(UserID userId) {
        var userPosts = _contents.Where(c => c.Creator.Id == userId).ToList();
        return Task.FromResult(Result<List<Content>>.Success(userPosts));
    }

    public Task<Result<PostId>> GetParentPostIdAsync(CommentId commentId) {
        var comment = _contents.OfType<Comment>().FirstOrDefault(c => c.Id == commentId);
        if (comment == null) return Task.FromResult(Result<PostId>.Fail(Error.ContentNotFound));

        return Task.FromResult(Result<PostId>.Success((comment.ParentPost.Id as PostId)!));
    }

    public Task<Result<List<Content>>> GetCommentsByUserIdAsync(UserID userId) {
        var userComments =
            _contents.OfType<Comment>().Where(c => c.Creator.Id == userId).Cast<Content>().ToList();
        return Task.FromResult(Result<List<Content>>.Success(userComments));
    }

    public Task<Result<List<Content>>> GetCommentsByPostIdAsync(ContentIDBase postId) {
        var comments = _contents.OfType<Comment>().Where(c => c.ParentPost.Id == postId).ToList();
        var commentCastedList = comments.Cast<Content>().ToList();
        return Task.FromResult(Result<List<Content>>.Success(commentCastedList));
    }

    public Task<Result<Content>> GetCommentByIdAsync(CommentId queryCommentId) {
        var comment = _contents.OfType<Comment>().FirstOrDefault(c => c.Id == queryCommentId);
        return Task.FromResult(comment != null
            ? Result<Content>.Success(comment)
            : Result<Content>.Fail(Error.ContentNotFound));
    }

    public Task<Result<Content>> GetCommentByIdWithAuthorAsync(CommentId queryCommentId, User user) {
        var comment = _contents.OfType<Comment>()
            .FirstOrDefault(c => c.Id == queryCommentId && c.Creator.Id == user.Id);
        return Task.FromResult(comment != null
            ? Result<Content>.Success(comment)
            : Result<Content>.Fail(Error.ContentNotFound));
    }

    public Task<Result<List<Content>>> GetAllPostThatUserCommentAsync(UserID userId) {
        var postsCommented = _contents.OfType<Comment>().Where(c => c.Creator.Id == userId).Select(c => c.ParentPost)
            .Distinct().ToList();
        return Task.FromResult(Result<List<Content>>.Success(postsCommented));
    }
}