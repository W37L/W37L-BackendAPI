using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Common.Repository;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Domain.Agregates.Post.Repository;

/// <summary>
/// Interface for interacting with content-related data in the application.
/// </summary>
public interface IContentRepository : IRepository<Content, ContentIDBase> {
    /// <summary>
    /// Retrieves the author ID of the content specified by its ID asynchronously.
    /// </summary>
    Task<Result<UserID>> GetAuthorIdAsync(ContentIDBase id);

    /// <summary>
    /// Retrieves content by its full ID asynchronously, considering the context of the user who is accessing the content.
    /// </summary>
    Task<Result<Content>> GetByFullIdAsync(ContentIDBase id, global::User user);

    /// <summary>
    /// Retrieves posts associated with a specific user asynchronously.
    /// </summary>
    Task<Result<List<Content>>> GetPostsByUserIdAsync(UserID userId);

    /// <summary>
    /// Retrieves the ID of the parent post of a comment asynchronously.
    /// </summary>
    Task<Result<PostId>> GetParentPostIdAsync(CommentId commentId);

    /// <summary>
    /// Retrieves comments made by a specific user asynchronously.
    /// </summary>
    Task<Result<List<Content>>> GetCommentsByUserIdAsync(UserID queryUserId);

    /// <summary>
    /// Retrieves comments associated with a specific post asynchronously.
    /// </summary>
    Task<Result<List<Content>>> GetCommentsByPostIdAsync(ContentIDBase postId);

    /// <summary>
    /// Retrieves a comment by its ID asynchronously.
    /// </summary>
    Task<Result<Content>> GetCommentByIdAsync(CommentId queryCommentId);

    /// <summary>
    /// Retrieves a comment by its ID asynchronously, considering the context of the user who is accessing the comment.
    /// </summary>
    Task<Result<Content>> GetCommentByIdWithAuthorAsync(CommentId queryCommentId, global::User user);

    /// <summary>
    /// Retrieves all posts that a user has commented on asynchronously.
    /// </summary>
    Task<Result<List<Content>>> GetAllPostThatUserCommentAsync(UserID userId);
}