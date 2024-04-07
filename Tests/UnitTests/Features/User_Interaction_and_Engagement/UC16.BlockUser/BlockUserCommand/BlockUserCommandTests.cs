using UnitTests.Common.Factories;
using W3TL.Core.Application.CommandDispatching.Commands.User;

public class BlockUserCommandTests {
    [Fact]
    public void Create_ValidArguments_ReturnsSuccess() {
        // Arrange
        var userId = UserFactory.InitWithDefaultValues().Build().Id.Value;
        var blockedUserId = UserFactory.InitWithDefaultValues().Build().Id.Value;

        // Act
        var result = BlockUserCommand.Create(userId, blockedUserId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(userId, result.Payload.Id.Value);
        Assert.Equal(blockedUserId, result.Payload.BlockedUserId.Value);
    }

    [Fact]
    public void Create_InvalidArguments_ReturnsFailure() {
        // Arrange
        var invalidUserId = "invalid";
        var invalidBlockedUserId = "invalid";

        // Act
        var result = BlockUserCommand.Create(invalidUserId, invalidBlockedUserId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.InvalidPrefix, result.Error.EnumerateAll());
    }
}