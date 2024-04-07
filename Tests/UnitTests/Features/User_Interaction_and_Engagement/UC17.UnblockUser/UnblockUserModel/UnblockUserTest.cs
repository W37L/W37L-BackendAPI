using UnitTests.Common.Factories;

namespace UnitTests.Features.User_Interaction_and_Engagement.UC17.UnblockUser;

public class UnblockUserTest {
    //ID: UC17 S1
    [Fact]
    public void Unblock_User_SuccessfullyUnblocks() {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();
        var userToUnblock = UserFactory.InitWithDefaultValues().Build();
        user.Block(userToUnblock);

        // Act
        var result = user.Unblock(userToUnblock);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.DoesNotContain(userToUnblock, user.Blocked);
    }


    //ID: UC17 F1
    [Fact]
    public void Unblock_User_NotBlocked_FailureMessageReturned() {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();
        var userToUnblock = UserFactory.InitWithDefaultValues().Build();

        // Act
        var result = user.Unblock(userToUnblock);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.UserNotBlocked.Message, result.Error.Message);
    }

    //ID: UC17 F2
    [Fact]
    public void Unblock_UserNull_FailureMessageReturned() {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();
        global::User userToUnblock = null;

        // Act
        var result = user.Unblock(userToUnblock);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.NullUser, result.Error.EnumerateAll());
    }
}