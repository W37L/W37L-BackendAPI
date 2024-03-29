using System.Net.Mime;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.Post.Enum;
using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Agregates.User.Entity.Values;

namespace W3TL.Core.Domain.Agregates.Post;

public class Post : Content {
    public MediaUrl MediaUrl { get; private set; }
    public ContentType ContentType { get; private set; }
    List<Comment>? Comments { get; set; }

    private Post(PostID postId, CreatedAtType createdAt, TheString contentTweet, Count likes, global::User creator, Signature signature, PostType postType, MediaUrl mediaUrl, ContentType contentType, List<Comment>? comments = null, Content? parentPost = null)
        : base(postId, createdAt, contentTweet, likes, creator, signature, postType, parentPost) {
        MediaUrl = mediaUrl;
        ContentType = contentType;
        Comments = comments;
    }

    public static Result<Post> Create(
        PostID postId,
        CreatedAtType createdAt,
        TheString contentTweet,
        Count likes,
        global::User creator,
        Signature signature,
        PostType postType,
        MediaUrl mediaUrl,
        ContentType contentType,
        List<Comment>? comments = null,
        Content? parentPost = null
    ) {
        try {
            var post = new Post(postId, createdAt, contentTweet, likes, creator, signature, postType, mediaUrl, contentType, comments, parentPost);
            return post;
        }
        catch (System.Exception ex) {
            return Error.FromException(ex);
        }
    }

}