using System.Net.Mime;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.Post.Enum;
using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Agregates.User.Entity.Values;

namespace W3TL.Core.Domain.Agregates.Post;

public class Post : Content {
    private Post(PostID postId, CreatedAtType createdAt, TheString contentTweet, Count likes, global::User creator, Signature signature, PostType postType, MediaUrl mediaUrl, ContentType contentType, Content? parentPost = null)
        : base(postId, createdAt, contentTweet, likes, creator, signature, postType, parentPost) {
        MediaUrl = mediaUrl;
        ContentType = contentType;
        Comments = new List<Comment>();
    }

    public MediaUrl MediaUrl { get; private set; }
    public ContentType ContentType { get; private set; }
    List<Comment>? Comments { get; set; }

    public static Result<Post> Create(
        TheString contentTweet,
        global::User creator,
        Signature signature,
        PostType postType,
        MediaUrl? mediaUrl = null,
        ContentType? contentType = null,
        Content? parentPost = null
    ) {
        try {
            HashSet<Error> errors = new();
            if (contentTweet is null) errors.Add(Error.NullContentTweet);
            if (creator is null) errors.Add(Error.NullCreator);
            if (signature is null) errors.Add(Error.NullSignature);
            if (postType == null) errors.Add(Error.NullPostType);
            if (errors.Any()) return Error.CompileErrors(errors);

            var postId = PostID.Generate().Payload;
            var createdAt = CreatedAtType.Create().Payload;
            var likes = Count.Zero;
            var post = new Post(postId, createdAt, contentTweet, likes, creator, signature, postType, mediaUrl!, contentType!, parentPost!);
            return post;
        }
        catch (Exception ex) {
            return Error.FromException(ex);
        }
    }

    public Result UpdateContentTweet(TheString contentTweet) {
        try {
            ContentTweet = contentTweet;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result AddComment(Comment comment) {
        try {
            Comments.Add(comment);
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result RemoveComment(Comment comment) {
        try {
            Comments.Remove(comment);
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result UpdateMediaUrl(MediaUrl mediaUrl) {
        try {
            MediaUrl = mediaUrl;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result UpdateContentType(ContentType contentType) {
        try {
            ContentType = contentType;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result UpdateComments(List<Comment> comments) {
        try {
            Comments = comments;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result UpdateLikes(Count likes) {
        try {
            Likes = likes;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}