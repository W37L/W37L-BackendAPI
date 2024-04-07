using UnitTests.Common.Factories;
using W3TL.Core.Domain.Agregates.User.Entity.Values;

public class UserCountUpdateTests {
    [Fact]
    public void IncrementFollowers_ShouldIncreaseFollowersCount() {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();
        var initialCount = user.Profile.Followers.Value;

        // Act
        var result = user.Profile.IncrementFollowers();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(initialCount + 1, user.Profile.Followers.Value);
    }

    [Fact]
    public void DecrementFollowers_ShouldDecreaseFollowersCount() {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();
        // Ensure there's at least one follower to decrement
        user.Profile.Followers = Count.Create(1).Payload;
        var initialCount = user.Profile.Followers.Value;

        // Act
        var result = user.Profile.DecrementFollowers();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(initialCount - 1, user.Profile.Followers.Value);
    }

    [Fact]
    public void IncrementFollowing_ShouldIncreaseFollowingCount() {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();
        var initialCount = user.Profile.Following.Value;

        // Act
        var result = user.Profile.IncrementFollowing();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(initialCount + 1, user.Profile.Following.Value);
    }

    [Fact]
    public void DecrementFollowing_ShouldDecreaseFollowingCount() {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();
        // Ensure there's at least one following to decrement
        user.Profile.Following = Count.Create(1).Payload;
        var initialCount = user.Profile.Following.Value;

        // Act
        var result = user.Profile.DecrementFollowing();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(initialCount - 1, user.Profile.Following.Value);
    }
}