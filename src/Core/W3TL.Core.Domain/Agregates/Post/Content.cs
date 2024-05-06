using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.Post.Enum;
using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Agregates.User.Entity.Values;
using W3TL.Core.Domain.Common.Bases;
using W3TL.Core.Domain.Services;

namespace W3TL.Core.Domain.Agregates.Post;

public abstract class Content : AggregateRoot<ContentIDBase> {
    //Constructor for the Test Factory
    internal Content(ContentIDBase contentId) : base(contentId) { }

    //Posible not used constructor
    protected Content(ContentIDBase postId, CreatedAtType createdAt, TheString contentTweet, Count likes,
        global::User creator, Signature signature, Content? parentPost = null) : base(postId) {
        CreatedAt = createdAt;
        ContentTweet = contentTweet;
        Likes = likes;
        Creator = creator;
        Signature = signature;
        PostType = PostType.Comment;
        ParentPost = parentPost;
    }

    public CreatedAtType CreatedAt { get; internal set; }
    public TheString ContentTweet { get; internal set; }
    public Count Likes { get; internal set; }
    public global::User Creator { get; internal set; }
    public Signature Signature { get; internal set; }
    public PostType PostType { get; internal set; }
    public Content? ParentPost { get; internal set; }

    public Result EditContent(TheString contentTweet, Signature signature) {
        if (contentTweet is null) return Error.NullContentTweet;
        if (signature is null) return Error.NullSignature;
        try {
            ContentTweet = contentTweet;
            Signature = signature;
            return Result.Ok;
        }
        catch (Exception ex) {
            return Error.FromException(ex);
        }
    }

    public Result Like(global::User liker) {
        return LikeService.Handle(liker, this);
    }

    public Result Unlike(global::User liker) {
        return UnlikeService.Handle(liker, this);
    }
}