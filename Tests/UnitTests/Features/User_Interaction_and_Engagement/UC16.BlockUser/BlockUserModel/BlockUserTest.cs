using UnitTests.Common.Factories;

namespace UnitTests.Features.User_Interaction_and_Engagement.UC16.BlockUser;

public class BlockUserTest {
    //ID: UC16 S1
    [Fact]
    public void Block_User_SuccessfullyBlocks() {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();
        var userToBlock = UserFactory.InitWithDefaultValues().Build();

        // Act
        var result = user.Block(userToBlock);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(userToBlock, user.Blocked);
    }


    //ID: UC16 F1
    [Fact]
    public void Block_User_AlreadyBlocked_FailureMessageReturned() {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();
        var userToBlock = UserFactory.InitWithDefaultValues().Build();
        user.Block(userToBlock);

        // Act
        var result = user.Block(userToBlock);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.UserAlreadyBlocked.Message, result.Error.Message);
    }

    //ID: UC16 F2
    [Fact]
    public void Block_UserNull_FailureMessageReturned() {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();
        global::User userToBlock = null;

        // Act
        var result = user.Block(userToBlock);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.NullUser, result.Error.EnumerateAll());
    }
}