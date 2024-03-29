using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.Post.Enum;
using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Agregates.User.Entity.Values;
using W3TL.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Agregates.Post;

public abstract class Content : AggregateRoot<PostID> {

    public CreatedAtType CreatedAt { get; private set; }
    public TheString ContentTweet { get; private set; }
    public Count Likes { get; private set; }
    public global::User Creator { get; private set; }
    public Signature Signature { get; private set; }
    public PostType PostType { get; private set; }
    public Content? ParentPost { get; private set; }

    // private Content(PostID postId, string value) : base(postId) {
    //     ContentTweet = ContentType.Create(value).Value;
    //     CreatedAt = CreatedAtType.Create().Value;
    //     Likes = Count.Zero;
    //     Signature = Signature.Create(value).Value;
    // }

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