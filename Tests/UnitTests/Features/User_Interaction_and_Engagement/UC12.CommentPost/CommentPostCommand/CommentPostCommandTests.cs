using UnitTests.Common.Factories;
using W3TL.Core.Application.CommandDispatching.Commands.Post;

public class CommentPostCommandTests {
    [Fact]
    public void Create_ValidArguments_ReturnsSuccess() {
        // Arrange
        var postId = PostFactory.InitWithDefaultValues().Build().Id.Value;
        var content = "This is a comment";
        var creatorId = UserFactory.InitWithDefaultValues().Build().Id.Value;
        var signature = ValidFields.VALID_SIGNATURE;
        var parentPostId = postId; // Assuming the comment is on the original post itself for simplicity

        // Act
        var result = CommentPostCommand.Create(postId, content, creatorId, signature, parentPostId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(content, result.Payload.Content.Value);
    }

    [Fact]
    public void Create_InvalidArguments_ReturnsFailure() {
        // Arrange
        var invalidPostId = ""; // Assuming this will cause PostId creation to fail
        var invalidContent = "";
        var invalidCreatorId = "";
        var invalidSignature = "";
        var invalidParentPostId = "";

        // Act
        var result = CommentPostCommand.Create(invalidPostId, invalidContent, invalidCreatorId, invalidSignature, invalidParentPostId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.BlankOrNullString, result.Error.EnumerateAll());
    }
}