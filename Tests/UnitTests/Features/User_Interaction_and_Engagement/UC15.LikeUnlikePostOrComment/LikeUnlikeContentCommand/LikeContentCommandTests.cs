using UnitTests.Common.Factories;
using W3TL.Core.Application.CommandDispatching.Commands.Post;

public class LikeContentCommandTests {
    [Fact]
    public void Create_ValidArguments_ReturnsSuccess() {
        // Arrange
        var postId = PostFactory.InitWithDefaultValues().Build().Id.Value;
        var likerId = UserFactory.InitWithDefaultValues().Build().Id.Value;

        // Act
        var result = LikeContentCommand.Create(postId, likerId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(postId, result.Payload.Id.Value);
        Assert.Equal(likerId, result.Payload.LikerId.Value);
    }

    [Fact]
    public void Create_InvalidArguments_ReturnsFailure() {
        // Arrange
        var invalidPostId = "";
        var invalidLikerId = "";

        // Act
        var result = LikeContentCommand.Create(invalidPostId, invalidLikerId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.InvalidPrefix, result.Error.EnumerateAll());
    }
}