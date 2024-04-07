using UnitTests.Common.Factories;
using W3TL.Core.Domain.Agregates.Post.Enum;
using W3TL.Core.Domain.Agregates.Post.Values;
using static UnitTests.Common.Factories.ValidFields;

namespace UnitTests.Features.Content_Management.UC12.CreateComment;

public class CommentCreationTestsSuccess {
    // Success scenarios for creating a comment

    // ID:UC12.S1
    [Fact]
    public void CreateComment_WithValidFields_ReturnSuccess() {
        // Arrange
        var contentTweet = TheString.Create(VALID_COMMENT_CONTENT).Payload;
        var creator = UserFactory.InitWithDefaultValues().Build();
        var signature = Signature.Create(VALID_SIGNATURE).Payload;
        var parentPost = PostFactory.InitWithDefaultValues().Build(); // Assuming a factory for creating a post

        // Act
        var result = Comment.Create(contentTweet, creator, signature, parentPost);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.Equal(contentTweet.Value, result.Payload.ContentTweet.Value);
        Assert.Equal(creator, result.Payload.Creator);
        Assert.Equal(signature.Value, result.Payload.Signature.Value);
        Assert.Equal(parentPost, result.Payload.ParentPost);
    }

    // ID:UC12.S2
    [Theory]
    [InlineData("Comment with a link: https://www.example.com")]
    [InlineData("Short comment")]
    [InlineData("Longer comment that still fits within the maximum allowed length for a comment. This should still be considered valid.")]
    public void CreateComment_VariousContentLengths_ReturnSuccess(string commentContent) {
        // Arrange
        var contentTweet = TheString.Create(commentContent).Payload;
        var creator = UserFactory.InitWithDefaultValues().Build();
        var signature = Signature.Create(VALID_SIGNATURE).Payload;
        var parentPost = PostFactory.InitWithDefaultValues().Build(); // Assuming a factory for creating a post
        var postType = PostType.Comment;

        // Act
        var result = Comment.Create(contentTweet, creator, signature, parentPost);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.Equal(commentContent, result.Payload.ContentTweet.Value);
    }


    // ID:UC12.S3
    // Test creating a comment with maximum allowed content length
    [Fact]
    public void CreateComment_MaxContentLength_ReturnSuccess() {
        // Arrange
        var content = new string('a', TheString.MAX_LENGTH); // Maximum length content
        var contentTweet = TheString.Create(content).Payload;
        var creator = UserFactory.InitWithDefaultValues().Build();
        var signature = Signature.Create(VALID_SIGNATURE).Payload;
        var parentPost = PostFactory.InitWithDefaultValues().Build(); // Assuming a factory for creating a post
        var postType = PostType.Comment;

        // Act
        var result = Comment.Create(contentTweet, creator, signature, parentPost);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(content, result.Payload.ContentTweet.Value);
    }


    // ID:UC12.S4
    // Test creating a comment to ensure it can be created with minimal required fields (content, creator, signature, parentPost)
    [Fact]
    public void CreateComment_MinimalRequiredFields_ReturnSuccess() {
        // Arrange
        var contentTweet = TheString.Create("Minimal content").Payload;
        var creator = UserFactory.InitWithDefaultValues().Build();
        var signature = Signature.Create(VALID_SIGNATURE).Payload;
        var parentPost = PostFactory.InitWithDefaultValues().Build(); // Original post as parent

        // Act
        var result = Comment.Create(contentTweet, creator, signature, parentPost);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(contentTweet.Value, result.Payload.ContentTweet.Value);
        Assert.Equal(creator, result.Payload.Creator);
        Assert.Equal(signature.Value, result.Payload.Signature.Value);
        Assert.Equal(parentPost, result.Payload.ParentPost);
    }
}