using System.Net.Mime;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Enum;
using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Agregates.User.Entity.Values;
using static UnitTests.Common.Factories.ValidFields;

namespace UnitTests.Common.Factories;

public class PostFactory {
    private Post _post;

    private PostFactory() { }

    public static PostFactory Init() {
        var factory = new PostFactory();
        var postId = PostID.Generate().Payload;
        factory._post = new Post(postId);
        return new PostFactory();
    }

    public static PostFactory InitWithDefaultValues() {
        var factory = new PostFactory();
        var contentTweet = TheString.Create(VALID_POST_CONTENT).Payload;
        var creator = UserFactory.InitWithDefaultValues().Build();
        var signature = Signature.Create(VALID_SIGNATURE).Payload;
        var postType = PostType.Original;

        var result = Post.Create(contentTweet, creator, signature, postType);

        factory._post = result.Payload;
        return factory;
    }

    public Post Build() {
        return _post;
    }

    public PostFactory WithValidId(string? uid) {
        _post.Id = PostID.Create(uid).Payload;
        return this;
    }

    public PostFactory WithValidContentTweet(string? contentTweet) {
        _post.ContentTweet = TheString.Create(contentTweet).Payload;
        return this;
    }

    public PostFactory WithValidCreator(User creator) {
        _post.Creator = creator;
        return this;
    }

    public PostFactory WithValidSignature(string? signature) {
        _post.Signature = Signature.Create(signature).Payload;
        return this;
    }

    public PostFactory WithValidPostType(PostType postType) {
        _post.PostType = postType;
        return this;
    }

    public PostFactory WithValidMediaUrl(string? mediaUrl) {
        _post.MediaUrl = MediaUrl.Create(mediaUrl).Payload;
        return this;
    }

    public PostFactory WithValidContentType(ContentType contentType) {
        _post.ContentType = contentType;
        return this;
    }

    public PostFactory WithValidParentPost(Post parentPost) {
        _post.ParentPost = parentPost;
        return this;
    }

    public PostFactory WithValidCreatedAt(string createdAt) {
        _post.CreatedAt = CreatedAtType.Create(createdAt).Payload;
        return this;
    }

    public PostFactory WithValidLikes(int likes) {
        _post.Likes = Count.Create(likes).Payload;
        return this;
    }
}