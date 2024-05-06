using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Agregates.User.Entity.Values;

public class Comment : Content {
    private Comment() : base(default!) { } //EF Core

    internal Comment(PostId postId) : base(postId) { }

    private protected Comment(CommentId commentId, CreatedAtType createdAt, TheString contentTweet, Count likes,
        User creator,
        Signature signature, Content parentPost)
        : base(commentId, createdAt, contentTweet, likes, creator, signature, parentPost) { }

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