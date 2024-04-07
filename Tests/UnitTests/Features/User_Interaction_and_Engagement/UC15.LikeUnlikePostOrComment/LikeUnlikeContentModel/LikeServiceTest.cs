using UnitTests.Common.Factories;
using W3TL.Core.Domain.Services;

namespace UnitTests.Features.Content_Management;

public class LikeServiceTests {
    // Success Scenario
    [Fact]
    public void Like_ValidParameters_Success() {
        var liker = UserFactory.InitWithDefaultValues().Build();

        var content = PostFactory
            .InitWithDefaultValues()
            .Build();

        var result = content.Like(liker);

        Assert.True(result.IsSuccess);
        Assert.Contains(content, liker.Likes);
        Assert.Equal(1, content.Likes.Value);
    }

    // Failure Scenarios
    [Fact]
    public void Like_NullUserOrContent_ReturnsError() {
        var result = LikeService.Handle(null, null);
        Assert.True(result.IsFailure);
        Assert.Contains(Error.NullUser, result.Error.EnumerateAll());
    }

    [Fact]
    public void Like_AlreadyLikedContent_ReturnsError() {
        var liker = UserFactory.InitWithDefaultValues().Build();
        var content = PostFactory.InitWithDefaultValues().Build();

        // Simulate already liked content
        liker.Likes.Add(content);

        var result = content.Like(liker);
        Assert.True(result.IsFailure);
        Assert.Contains(Error.UserAlreadyLiked, result.Error.EnumerateAll());
    }

    [Fact]
    public void Like_BlockedUser_ReturnsError() {
        var liker = UserFactory.InitWithDefaultValues().Build();
        var contentCreator = UserFactory.InitWithDefaultValues().Build();
        var content = PostFactory
            .InitWithDefaultValues()
            .WithValidCreator(contentCreator)
            .Build();

        // Simulate blocked relationship
        contentCreator.Block(liker);

        var result = content.Like(liker);
        Assert.True(result.IsFailure);
        Assert.Contains(Error.UserBlocked, result.Error.EnumerateAll());
    }
}