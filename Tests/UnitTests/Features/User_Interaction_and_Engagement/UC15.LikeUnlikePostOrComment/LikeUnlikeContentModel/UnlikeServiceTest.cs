using UnitTests.Common.Factories;

namespace UnitTests.Features.Content_Management;

public class UnlikeServiceTests {
    // Success Scenario
    [Fact]
    public void Unlike_ValidParameters_Success() {
        var unliker = UserFactory.InitWithDefaultValues().Build();
        var content = PostFactory.InitWithDefaultValues().Build();

        // Simulate like before unlike
        unliker.Interactions.Likes.Add(content.Id as PostId);
        content.Likes.Increment();

        var result = content.Unlike(unliker);

        Assert.True(result.IsSuccess);
        Assert.DoesNotContain(content.Id, unliker.Interactions.Likes);
        Assert.Equal(0, content.Likes.Value);
    }

    // Failure Scenarios
    [Fact]
    public void Unlike_NotLikedContent_ReturnsError() {
        var unliker = UserFactory.InitWithDefaultValues().Build();
        var content = PostFactory.InitWithDefaultValues().Build();

        var result = content.Unlike(unliker);

        Assert.True(result.IsFailure);
        Assert.Contains(Error.UserNotLiked, result.Error.EnumerateAll());
    }
}