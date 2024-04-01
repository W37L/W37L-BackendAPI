using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Agregates.User.Entity.Values;

public class Comment : Content {
    internal Comment(PostId postId) : base(postId) { }

    private protected Comment(PostId postId, CreatedAtType createdAt, TheString contentTweet, Count likes, User creator, Signature signature, Content parentPost)
        : base(postId, createdAt, contentTweet, likes, creator, signature, parentPost) { }

    public static Result<Comment> Create(
        TheString contentTweet,
        User creator,
        Signature signature,
        Content parentPost
    ) {
        try {
            HashSet<Error> errors = new();
            if (contentTweet is null) errors.Add(Error.NullContentTweet);
            if (creator is null) errors.Add(Error.NullCreator);
            if (signature is null) errors.Add(Error.NullSignature);
            if (parentPost is null) errors.Add(Error.NullParentPost);

            if (errors.Any()) return Error.CompileErrors(errors);

            var postId = PostId.Generate().Payload;
            var createdAt = CreatedAtType.Create().Payload;
            var likes = Count.Zero;
            var comment = new Comment(postId, createdAt, contentTweet, likes, creator, signature, parentPost);
            return comment;
        }
        catch (Exception ex) {
            return Error.FromException(ex);
        }
    }
}