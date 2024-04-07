using UnitTests.Common.Factories;
using W3TL.Core.Application.CommandDispatching.Commands.Post;

public class UpdateCommentCommandTests {
    [Fact]
    public void Create_ValidArguments_ReturnsSuccess() {
        // Arrange
        var postId = PostFactory.InitWithDefaultValues().Build().Id.Value;
        var newContent = "Updated comment content";
        var signature = ValidFields.VALID_SIGNATURE;

        // Act
        var result = UpdateContentCommand.Create(postId, newContent, signature);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newContent, result.Payload.ContentTweet.Value);
        Assert.Equal(signature, result.Payload.Signature.Value);
    }

    [Fact]
    public void Create_InvalidArguments_ReturnsFailure() {
        // Arrange
        var invalidPostId = "";
        var invalidContent = "";
        var invalidSignature = "";

        // Act
        var result = UpdateContentCommand.Create(invalidPostId, invalidContent, invalidSignature);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.InvalidPrefix, result.Error.EnumerateAll());
    }
}