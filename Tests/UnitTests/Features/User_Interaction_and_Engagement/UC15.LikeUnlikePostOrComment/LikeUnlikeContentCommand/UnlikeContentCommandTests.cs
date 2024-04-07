using UnitTests.Common.Factories;
using W3TL.Core.Application.CommandDispatching.Commands.Post;

public class UnlikeContentCommandTests {
    [Fact]
    public void Create_ValidArguments_ReturnsSuccess() {
        // Arrange
        var postId = PostFactory.InitWithDefaultValues().Build().Id.Value;
        var unlikerId = UserFactory.InitWithDefaultValues().Build().Id.Value;

        // Act
        var result = UnlikeContentCommand.Create(postId, unlikerId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(postId, result.Payload.Id.Value);
        Assert.Equal(unlikerId, result.Payload.UnlikerId.Value);
    }

    [Fact]
    public void Create_InvalidArguments_ReturnsFailure() {
        // Arrange
        var invalidPostId = "";
        var invalidUnlikerId = "";

        // Act
        var result = UnlikeContentCommand.Create(invalidPostId, invalidUnlikerId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.InvalidPrefix, result.Error.EnumerateAll());
    }
}