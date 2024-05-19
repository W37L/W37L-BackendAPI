using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Agregates.User.Entity.Values;

///<summary>
///  Represents the content of a comment.
/// </summary>
public class Comment : Content {
    private Comment() : base(default!) { } //EF Core

    internal Comment(PostId postId) : base(postId) { }

    private protected Comment(CommentId commentId, CreatedAtType createdAt, TheString contentTweet, Count likes,
        User creator,
        Signature signature, Content parentPost)
        : base(commentId, createdAt, contentTweet, likes, creator, signature, parentPost) { }

    /// <summary>
    /// Creates a new instance of the <see cref="Comment"/> class.
    /// </summary>
    /// <param name="contentTweet">The content of the comment.</param>
    /// <param name="creator">The user who created the comment.</param>
    /// <param name="signature">The signature of the comment.</param>
    /// <param name="parentPost">The parent post of the comment.</param>
    /// <returns>A result indicating success or failure with the created comment.</returns>
    public static Result<Comment> Create(
        TheString contentTweet,
        User creator,
        Signature signature,
        Content parentPost
    ) {
        ArgumentNullException.ThrowIfNull(contentTweet);
        ArgumentNullException.ThrowIfNull(creator);
        ArgumentNullException.ThrowIfNull(signature);
        ArgumentNullException.ThrowIfNull(parentPost);

        HashSet<Error> errors = new();
        var commentId = CommentId.Generate()
            .OnFailure(error => errors.Add(error));

        var createdAt = CreatedAtType.Create()
            .OnFailure(error => errors.Add(error));

        var likes = Count.Zero;

        if (errors.Any())
            return Error.CompileErrors(errors);

        var comment = new Comment(commentId.Payload, createdAt.Payload, contentTweet, likes!, creator, signature,
            parentPost);
        return comment;
    }
}