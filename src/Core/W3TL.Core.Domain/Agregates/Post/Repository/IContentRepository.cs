using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Common.Repository;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Domain.Agregates.Post.Repository;

public interface IContentRepository : IRepository<Content, ContentIDBase> {
    Task<Result<UserID>> GetAuthorIdAsync(ContentIDBase id);
    Task<Result<Content>> GetByFullIdAsync(ContentIDBase id, global::User user);
    Task<Result<List<Content>>> GetPostsByUserIdAsync(UserID userId);

    Task<Result<PostId>> GetParentPostIdAsync(CommentId commentId);

    Task<Result<List<Content>>> GetCommentsByUserIdAsync(UserID queryUserId);
    Task<Result<List<Content>>> GetCommentsByPostIdAsync(ContentIDBase postId);
    Task<Result<Content>> GetCommentByIdAsync(CommentId queryCommentId);
    Task<Result<Content>> GetCommentByIdWithAuthorAsync(CommentId queryCommentId, global::User user);
    Task<Result<List<Content>>> GetAllPostThatUserCommentAsync(UserID userId);
}