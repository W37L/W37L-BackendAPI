using UnitTests.Common.Factories;
using Xunit;

namespace UnitTests.Features.User_Interaction_and_Engagement;

public class UserFollowTests {

    private readonly global::User _follower;
    private readonly global::User _followee;

    public UserFollowTests() {
        _follower = UserFactory.InitWithDefaultValues().Build();
        _followee = UserFactory.InitWithDefaultValues()
            .WitValidId("UID-123456789012345678901234567890123456")
            .WithValidUserName("username")
            .WithValidEmail("email@example.com")
            .Build();
    }

    private void ResetFollowState() {
        _follower.Following.Clear();
        _followee.Followers.Clear();
        _follower.Blocked.Clear();
        _followee.Blocked.Clear();
        // Additional reset logic if necessary
    }

    // Success Scenario

    // ID:UC6.S1
    [Fact]
    public void Follow_ValidUser_ShouldFollowSuccessfully() {
        // Arrange
        ResetFollowState();

        // Act
        var result = _follower.Follow(_followee);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(_followee, _follower.Following);
        Assert.Contains(_follower, _followee.Followers);
    }

    // Failure Scenarios

    // ID:UC6.F1
    [Fact]
    public void Follow_Self_ShouldReturnCannotFollowSelfError() {
        // Arrange
        ResetFollowState();

        // Act
        var result = _follower.Follow(_follower);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.CannotFollowSelf, result.Error);
    }

    // ID:UC6.F2
    [Fact]
    public void Follow_NullUser_ShouldReturnNullUserError() {
        // Arrange
        ResetFollowState();

        // Act
        var result = _follower.Follow(null);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.NullUser, result.Error);
    }

    // ID:UC6.F3
    [Fact]
    public void Follow_BlockedUser_ShouldReturnUserBlockedError() {
        // Arrange
        ResetFollowState();
        _follower.Block(_followee);

        // Act
        var result = _follower.Follow(_followee);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.UserBlocked, result.Error);
    }

    // ID:UC6.F4
    [Fact]
    public void Follow_AlreadyFollowedUser_ShouldReturnUserAlreadyFollowedError() {
        // Arrange
        ResetFollowState();
        _follower.Follow(_followee); // First follow attempt

        // Act
        var result = _follower.Follow(_followee); // Second follow attempt

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.UserAlreadyFollowed, result.Error);
    }
}
