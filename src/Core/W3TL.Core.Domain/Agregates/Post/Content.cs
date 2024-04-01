using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.Post.Enum;
using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Agregates.User.Entity.Values;
using W3TL.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Agregates.Post;

public abstract class Content : AggregateRoot<PostID> {
    //Constructor for the Test Factory
    internal Content(PostID postId) : base(postId) { }

    //Posible not used constructor
    protected Content(PostID postId, CreatedAtType createdAt, TheString contentTweet, Count likes, global::User creator, Signature signature, PostType postType, Content? parentPost = null) : base(postId) {
        CreatedAt = createdAt;
        ContentTweet = contentTweet;
        Likes = likes;
        Creator = creator;
        Signature = signature;
        PostType = postType;
        ParentPost = parentPost;
    }

    public CreatedAtType CreatedAt { get; internal set; }
    public TheString ContentTweet { get; internal set; }
    public Count Likes { get; internal set; }
    public global::User Creator { get; internal set; }
    public Signature Signature { get; internal set; }
    public PostType PostType { get; internal set; }
    public Content? ParentPost { get; internal set; }
}