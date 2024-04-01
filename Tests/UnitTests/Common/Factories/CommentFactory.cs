using W3TL.Core.Domain.Agregates.Post.Values;
using static UnitTests.Common.Factories.ValidFields;

namespace UnitTests.Common.Factories;

public class CommentFactory {
    private Comment _comment;

    private CommentFactory() { }

    public static CommentFactory Init() {
        var factory = new CommentFactory();
        var commentId = PostId.Generate().Payload;
        factory._comment = new Comment(commentId);
        return new CommentFactory();
    }

    public static CommentFactory InitWithDefaultValues() {
        var factory = new CommentFactory();
        var parentPost = PostFactory.InitWithDefaultValues().Build();
        var content = TheString.Create(VALID_COMMENT_CONTENT).Payload;
        var creator = UserFactory.InitWithDefaultValues().Build();
        var signature = Signature.Create(VALID_SIGNATURE).Payload;

        var result = Comment.Create(content, creator, signature, parentPost);

        factory._comment = result.Payload;
        return factory;
    }

    public Comment Build() {
        return _comment;
    }

    public CommentFactory WithValidId(string? uid) {
        _comment.Id = CommentId.Create(uid).Payload;
        return this;
    }

    public CommentFactory WithValidContent(string? content) {
        _comment.ContentTweet = TheString.Create(content).Payload;
        return this;
    }

    public CommentFactory WithValidCreator(User creator) {
        _comment.Creator = creator;
        return this;
    }

    public CommentFactory WithValidSignature(string signature) {
        _comment.Signature = Signature.Create(signature).Payload;
        return this;
    }
}