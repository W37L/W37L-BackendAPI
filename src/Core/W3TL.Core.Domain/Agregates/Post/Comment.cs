using System.Net.Mime;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Enum;
using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Agregates.User;
using W3TL.Core.Domain.Agregates.User.Entity.Values;

public class Comment : Content {
    private protected Comment(PostID postId, CreatedAtType createdAt, TheString contentTweet, Count likes, User creator, Signature signature, PostType postType, Content parentPost)
        : base(postId, createdAt, contentTweet, likes, creator, signature, postType, parentPost) { }

    public static Result<Comment> Create(
        PostID postId,
        CreatedAtType createdAt,
        TheString contentTweet,
        Count likes,
        User creator,
        Signature signature,
        PostType postType,
        Content parentPost
    ) {
        try {
            var comment = new Comment(postId, createdAt, contentTweet, likes, creator, signature, postType, parentPost);
            return comment;
        }
        catch (System.Exception ex) {
            return Error.FromException(ex);
        }
    }
}