using UnitTests.Common.Factories;
using W3TL.Core.Application.CommandDispatching.Commands.User;

public class UpdateAvatarUserCommandTests {
    [Fact]
    public void Create_ValidInput_Success() {
        // Arrange
        var userId = ValidFields.VALID_USER_ID;
        var avatarUrl = ValidFields.VALID_AVATAR_URL;

        // Act
        var result = UpdateAvatarUserCommand.Create(userId, avatarUrl);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(avatarUrl, result.Payload.Avatar.Url);
    }

    [Fact]
    public void Create_InvalidInput_Failure() {
        // Arrange
        var userId = "InvalidUserId"; // Assuming this will fail UserID creation
        var avatarUrl = ""; // Invalid because it's empty

        // Act
        var result = UpdateAvatarUserCommand.Create(userId, avatarUrl);

        // Assert
        Assert.True(result.IsFailure);
        // Since multiple errors could be added, we check if at least one is present
        Assert.NotEmpty(result.Error.EnumerateAll());
    }
}