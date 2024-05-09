using UnitTests.Common.Factories;

namespace UnitTests.Features.User_Interaction_and_Engagement;

public class UserUnfollowTests {
    private readonly global::User _followee = UserFactory.InitWithDefaultValues()
        .WitValidId("UID-123456789012345678901234567890123456")
        .WithValidUserName("username")
        .WithValidEmail("email@example.com")
        .Build();

    private readonly global::User _follower = UserFactory.InitWithDefaultValues().Build();

    private void ResetFollowState() {
        _follower.Interactions.Following.Clear();
        _followee.Interactions.Followers.Clear();
    }

    // Success Scenario

    // ID:UC7.S1
    [Fact]
    public void Unfollow_ValidUser_ShouldUnfollowSuccessfully() {
        // Arrange - Ensure they are following each other
        ResetFollowState();
        _follower.Follow(_followee);
        var initialValueFollower = _follower.Profile.Following.Value;
        var initialValueFollowee = _followee.Profile.Followers.Value;

        // Act
        var result = _follower.Unfollow(_followee);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.DoesNotContain(_followee.Id, _follower.Interactions.Following);
        Assert.DoesNotContain(_follower.Id, _followee.Interactions.Followers);
        Assert.Equal(initialValueFollower - 1, _follower.Profile.Following.Value);
        Assert.Equal(initialValueFollowee - 1, _followee.Profile.Followers.Value);
    }

    // Failure Scenarios

    // ID:UC7.F1
    [Fact]
    public void Unfollow_NotFollowedUser_ShouldReturnUserNotFollowedError() {
        // Arrange - Reset state to ensure they are not following each other
        ResetFollowState();
        var initialValueFollower = _follower.Profile.Following.Value;
        var initialValueFollowee = _followee.Profile.Followers.Value;

        // Act
        var result = _follower.Unfollow(_followee);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.UserNotFollowed, result.Error);
        Assert.DoesNotContain(_followee.Id, _follower.Interactions.Following);
        Assert.DoesNotContain(_follower.Id, _followee.Interactions.Followers);
        Assert.Equal(initialValueFollower, _follower.Profile.Following.Value);
        Assert.Equal(initialValueFollowee, _followee.Profile.Followers.Value);
    }

    // ID:UC7.F2
    [Fact]
    public void Unfollow_NullUser_ShouldReturnNullUserError() {
        // Arrange
        ResetFollowState();
        var initialValueFollower = _follower.Profile.Following.Value;
        var initialValueFollowee = _followee.Profile.Followers.Value;

        // Act
        var result = _follower.Unfollow(null);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.NullUser, result.Error);
        Assert.DoesNotContain(_followee.Id, _follower.Interactions.Following);
        Assert.DoesNotContain(_follower.Id, _followee.Interactions.Followers);
        Assert.Equal(initialValueFollower, _follower.Profile.Following.Value);
        Assert.Equal(initialValueFollowee, _followee.Profile.Followers.Value);
    }

    // Considering "Error.FromException" is a catch-all for unexpected errors, it's tricky to simulate directly in a unit test
    // because it relies on an exception being thrown during the operation. Such scenarios might include external system failures,
    // like a database error, that are beyond the scope of unit testing and into integration testing territory.

    // Additional scenarios like attempting to unfollow when the follower or followee list is somehow corrupted (leading to exceptions)
    // would also fall under more integration or end-to-end testing, as they involve more complex setups and external dependencies.
}