using UnitTests.Common.Factories;
using W3TL.Core.Domain.Services;

namespace UnitTests.Features.Content_Management;

public class UnlikeServiceTests {
    // Success Scenario
    [Fact]
    public void Unlike_ValidParameters_Success() {
        var unliker = UserFactory.InitWithDefaultValues().Build();
        var content = PostFactory.InitWithDefaultValues().Build();

        // Simulate like before unlike
        unliker.Likes.Add(content);
        content.Likes.Increment();

        var result = UnlikeService.Handle(unliker, content);

        Assert.True(result.IsSuccess);
        Assert.DoesNotContain(content, unliker.Likes);
        Assert.Equal(0, content.Likes.Value);
    }

    // Failure Scenarios
    [Fact]
    public void Unlike_NotLikedContent_ReturnsError() {
        var unliker = UserFactory.InitWithDefaultValues().Build();
        var content = PostFactory.InitWithDefaultValues().Build();

        var result = UnlikeService.Handle(unliker, content);

        Assert.True(result.IsFailure);
        Assert.Contains(Error.UserNotLiked, result.Error.EnumerateAll());
    }
}