using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Enum;
using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Agregates.User.Entity.Values;

///<summary>
///  Represents the content of a post.
/// </summary>
public class Post : Content {
    private Post() : base(default) { } //EF Core

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
    
    /// <summary>
    /// Gets or sets the media URL of the post.
    /// </summary>
    public MediaUrl MediaUrl { get; internal set; }

    /// <summary>
    /// Gets or sets the media type of the post.
    /// </summary>
    public MediaType MediaType { get; internal set; }

    /// <summary>
    /// Gets or sets the type of the post.
    /// </summary>
    public PostType PostType { get; internal set; }

    /// <summary>
    /// Gets or sets the comments associated with the post.
    /// </summary>
    public List<Comment>? Comments { get; set; }

    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="contentTweet">The content of the post.</param>
    /// <param name="creator">The creator of the post.</param>
    /// <param name="signature">The signature of the post.</param>
    /// <param name="postType">The type of the post.</param>
    /// <param name="mediaUrl">The media URL of the post.</param>
    /// <param name="mediaType">The media type of the post.</param>
    /// <param name="parentPost">The parent post, if any.</param>
    /// <returns>A result indicating success or failure.</returns>
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

    /// <summary>
    /// Creates a new post with the specified ID.
    /// </summary>
    /// <param name="pId">The ID of the post.</param>
    /// <param name="contentTweet">The content of the post.</param>
    /// <param name="creator">The creator of the post.</param>
    /// <param name="signature">The signature of the post.</param>
    /// <param name="postType">The type of the post.</param>
    /// <param name="mediaUrl">The media URL of the post.</param>
    /// <param name="mediaType">The media type of the post.</param>
    /// <param name="parentPost">The parent post, if any.</param>
    /// <returns>A result indicating success or failure.</returns>
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


    /// <summary>
    /// Updates the media URL of the post.
    /// </summary>
    /// <param name="mediaUrl">The new media URL.</param>
    /// <returns>A result indicating success or failure.</returns>
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

    /// <summary>
    /// Updates the content type of the post.
    /// </summary>
    /// <param name="mediaType">The new media type.</param>
    /// <param name="mediaType">The new media type.</param>
    /// <returns>A result indicating success or failure.</returns>
    public Result UpdateContentType(MediaType mediaType) {
        try {
            MediaType = mediaType;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    /// <summary>
    /// Adds a comment to the post.
    /// </summary>
    /// <param name="comment">The comment to add.</param>
    /// <returns>A result indicating success or failure.</returns>
    public Result AddComment(Comment comment) {
        if (comment is null) return Error.NullComment;
        try {
            if (Comments is null) Comments = new List<Comment>();
            Comments.Add(comment);
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    /// <summary>
    /// Removes a comment from the post.
    /// </summary>
    /// <param name="comment">The comment to remove.</param>
    /// <returns>A result indicating success or failure.</returns>
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

    /// <summary>
    /// Highlights the post for a user.
    /// </summary>
    /// <param name="user">The user who wants to highlight the post.</param>
    /// <returns>A result indicating success or failure.</returns>
    public Result Highlight(User user) {
        if (user is null) return Error.NullUser;
        try {
            user.Interactions.Highlights.Add(this.Id as PostId);
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    /// <summary>
    /// Removes the highlight from the post for a user.
    /// </summary>
    /// <param name="user">The user who wants to remove the highlight from the post.</param>
    /// <returns>A result indicating success or failure.</returns>
    public Result Unhighlight(User user) {
        if (user is null) return Error.NullUser;
        try {
            user.Interactions.Highlights.Remove(this.Id as PostId);
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    /// <summary>
    /// Retweets the post for a user.
    /// </summary>
    /// <param name="user">The user who wants to retweet the post.</param>
    /// <returns>A result indicating success or failure.</returns>
    public Result Retweet(User user) {
        if (user is null) return Error.NullUser;
        try {
            user.Interactions.RetweetedTweets.Add(this.Id as PostId);
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    /// <summary>
    /// Removes the retweet from the post for a user.
    /// </summary>
    /// <param name="user">The user who wants to remove the retweet from the post.</param>
    /// <returns>A result indicating success or failure.</returns>
    public Result Unretweet(User user) {
        if (user is null) return Error.NullUser;
        try {
            user.Interactions.RetweetedTweets.Remove(this.Id as PostId);
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}