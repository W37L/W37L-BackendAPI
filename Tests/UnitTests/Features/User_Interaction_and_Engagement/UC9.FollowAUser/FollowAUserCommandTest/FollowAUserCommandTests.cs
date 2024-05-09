using UnitTests.Common.Factories;
using W3TL.Core.Application.CommandDispatching.Commands.User;

public class FollowAUserCommandTests {
    [Fact]
    public void Create_ValidArguments_ReturnsSuccess() {
        // Arrange
        var validUserId = ValidFields.VALID_USER_ID;
        var validUserToFollowId = UserFactory.InitWithDefaultValues().Build().Id.Value;

        // Act
        var result = FollowAUserCommand.Create(validUserId, validUserToFollowId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validUserId, result.Payload.UserId.Value);
        Assert.Equal(validUserToFollowId, result.Payload.UserToFollowId.Value);
    }

    [Fact]
    public void Create_InvalidArguments_ReturnsFailure() {
        // Arrange
        var invalidUserId = ""; // Assuming this will cause UserID creation to fail
        var invalidUserToFollowId = "";

        // Act
        var result = FollowAUserCommand.Create(invalidUserId, invalidUserToFollowId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.InvalidFormat, result.Error.EnumerateAll());
    }
}