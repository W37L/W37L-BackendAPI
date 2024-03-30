using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.Post.Enum;
using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Agregates.User.Entity.Values;
using W3TL.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Agregates.Post;

public abstract class Content : AggregateRoot<PostID> {

    public CreatedAtType CreatedAt { get; protected set; }
    public TheString ContentTweet { get; protected set; }
    public Count Likes { get; protected set; }
    public global::User Creator { get; protected set; }
    public Signature Signature { get; protected set; }
    public PostType PostType { get; protected set; }
    public Content? ParentPost { get; protected set; }

    protected Content(PostID postId, CreatedAtType createdAt, TheString contentTweet, Count likes, global::User creator, Signature signature, PostType postType, Content? parentPost = null) : base(postId) {
        CreatedAt = createdAt;
        ContentTweet = contentTweet;
        Likes = likes;
        Creator = creator;
        Signature = signature;
        PostType = postType;
        ParentPost = parentPost;
    }

}