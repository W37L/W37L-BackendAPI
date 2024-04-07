using UnitTests.Common.Factories;
using W3TL.Core.Application.CommandDispatching.Commands.User;

public class UnblockUserCommandTests {
    [Fact]
    public void Create_ValidArguments_ReturnsSuccess() {
        // Arrange
        var userId = UserFactory.InitWithDefaultValues().Build().Id.Value;
        var unblockedUserId = ValidFields.VALID_USER_ID; // Ensure this is another valid user ID

        // Act
        var result = UnblockUserCommand.Create(userId, unblockedUserId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(userId, result.Payload.Id.Value);
        Assert.Equal(unblockedUserId, result.Payload.UnblockedUserId.Value);
    }

    [Fact]
    public void Create_InvalidArguments_ReturnsFailure() {
        // Arrange
        var invalidUserId = "Invalid"; // Invalid user ID
        var invalidUnblockedUserId = "Invalid"; // Another invalid user ID

        // Act
        var result = UnblockUserCommand.Create(invalidUserId, invalidUnblockedUserId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.InvalidPrefix, result.Error.EnumerateAll());
    }
}