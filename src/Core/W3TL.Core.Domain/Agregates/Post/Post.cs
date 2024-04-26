using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Enum;
using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Agregates.User.Entity.Values;

public class Post : Content {
    //Constructor for the Test Factory
    internal Post(PostId postId) : base(postId) { }


    private Post(PostId postId, CreatedAtType createdAt, TheString contentTweet, Count likes, User creator,
        Signature signature, PostType postType, MediaUrl mediaUrl, MediaType mediaType, Content? parentPost = null)
        : base(postId, createdAt, contentTweet, likes, creator, signature, parentPost) {
        MediaUrl = mediaUrl;
        MediaType = mediaType;
        PostType = postType;
        Comments = new List<Comment>();
    }

    public MediaUrl MediaUrl { get; internal set; }
    public MediaType MediaType { get; internal set; }
    public PostType PostType { get; internal set; }
    public List<Comment>? Comments { get; set; }

    public static Result<Post> Create(
        TheString contentTweet,
        User creator,
        Signature signature,
        PostType postType,
        MediaUrl? mediaUrl = null,
        MediaType mediaType = MediaType.Text,
        Content? parentPost = null
    ) {
        return Create(null, contentTweet, creator, signature, postType, mediaUrl, mediaType, parentPost);
    }

    public static Result<Post> Create(
        PostId pId,
        TheString contentTweet,
        User creator,
        Signature signature,
        PostType postType,
        MediaUrl? mediaUrl = null,
        MediaType mediaType = MediaType.Text,
        Content? parentPost = null
    ) {
        try {
            HashSet<Error> errors = new();
            if (contentTweet is null) errors.Add(Error.NullContentTweet);
            if (creator is null) errors.Add(Error.NullCreator);
            if (signature is null) errors.Add(Error.NullSignature);
            if (postType == null) errors.Add(Error.NullPostType);
            if (errors.Any()) return Error.CompileErrors(errors);

            var postId = pId == null ? PostId.Generate().Payload : pId;
            var createdAt = CreatedAtType.Create().Payload;
            var likes = Count.Zero;
            var post = new Post(postId, createdAt, contentTweet, likes, creator, signature, postType, mediaUrl!,
                mediaType, parentPost!);
            return post;
        }
        catch (Exception ex) {
            return Error.FromException(ex);
        }
    }


    public Result UpdateMediaUrl(MediaUrl mediaUrl) {
        if (mediaUrl is null) return Error.NullMediaUrl;
        try {
            MediaUrl = mediaUrl;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result UpdateContentType(MediaType mediaType) {
        try {
            MediaType = mediaType;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result AddComment(Comment comment) {
        if (comment is null) return Error.NullComment;
        try {
            Comments.Add(comment);
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result RemoveComment(Comment comment) {
        if (comment is null) return Error.NullComment;
        try {
            Comments.Remove(comment);
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}