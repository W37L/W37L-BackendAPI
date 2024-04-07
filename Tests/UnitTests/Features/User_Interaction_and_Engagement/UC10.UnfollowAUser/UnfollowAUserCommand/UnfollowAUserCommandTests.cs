using UnitTests.Common.Factories;
using W3TL.Core.Application.CommandDispatching.Commands.User;

public class UnfollowAUserCommandTests {
    [Fact]
    public void Create_ValidArguments_ReturnsSuccess() {
        // Arrange
        var validUserId = ValidFields.VALID_USER_ID;
        var validUserToUnFollowId = UserFactory.InitWithDefaultValues().Build().Id.Value;

        // Act
        var result = UnfollowAUserCommand.Create(validUserId, validUserToUnFollowId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validUserId, result.Payload.UserId.Value);
        Assert.Equal(validUserToUnFollowId, result.Payload.UserToUnFollowId.Value);
    }

    [Fact]
    public void Create_InvalidArguments_ReturnsFailure() {
        // Arrange
        var invalidUserId = ""; // Assuming this will cause UserID creation to fail
        var invalidUserToUnFollowId = "";

        // Act
        var result = UnfollowAUserCommand.Create(invalidUserId, invalidUserToUnFollowId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.InvalidPrefix, result.Error.EnumerateAll());
    }
}