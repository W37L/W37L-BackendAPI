using UnitTests.Common.Factories;
using W3TL.Core.Domain.Agregates.Post.Values;

namespace UnitTests.Features.Content_Management.UpdatePost;

public class PostUpdateContentTweetTests {
    // Expanded Success Scenarios

    // ID:UC7.S1
    [Theory]
    [InlineData("This is a valid tweet content.")]
    [InlineData("Another valid tweet, with a maximum length of 140 characters. Let's make sure it fits all the way to the end without any issues.")]
    public void UpdateContentTweet_ValidContent_ReturnSuccess(string? validContent) {
        // Arrange
        var post = PostFactory.InitWithDefaultValues().Build();
        var newContentTweet = TheString.Create(validContent).Payload;

        // Act
        var result = post.UpdateContentTweet(newContentTweet);

        // Assert
        Assert.True(result.IsSuccess);
    }

    // Expanded Failure Scenarios
    //ID:UC7.S2
    [Theory]
    [InlineData(140)]
    [InlineData(139)]
    [InlineData(1)]
    public void UpdateContentTweet_WithContentTweetInInt_ReturnSuccess(int length) {
        //Arrange
        var post = PostFactory.InitWithDefaultValues().Build();
        var contentTweet = new string('a', length);
        var newContentTweet = TheString.Create(contentTweet).Payload;

        //Act
        var result = post.UpdateContentTweet(newContentTweet);

        //Assert
        Assert.True(result.IsSuccess);
    }

    // ID:UC7.F1
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void UpdateContentTweet_BlankContent_ReturnFailure(string? invalidContent) {
        // Arrange
        var post = PostFactory.InitWithDefaultValues().Build();
        var newContentResult = TheString.Create(invalidContent);

        // Act
        Assert.True(newContentResult.IsFailure);

        // Assert
        Assert.Contains(Error.BlankOrNullString, newContentResult.Error.EnumerateAll());
    }

    // ID:UC7.F2
    [Fact]
    public void UpdateContentTweet_ContentTooLong_ReturnFailure() {
        // Arrange
        var post = PostFactory.InitWithDefaultValues().Build();
        var invalidContent = new string('a', TheString.MAX_LENGTH + 1); // Exceeding max length

        // Act
        var newContentResult = TheString.Create(invalidContent);

        // Assert
        Assert.True(newContentResult.IsFailure);
        Assert.Contains(Error.TooLongString(TheString.MAX_LENGTH), newContentResult.Error.EnumerateAll());
    }

    // ID:UC7.F3
    [Theory]
    [InlineData(141)]
    [InlineData(142)]
    [InlineData(150)]
    public void UpdateContentTweet_ContentTooLongWithInteger_ReturnFailure(int length) {
        // Arrange
        var post = PostFactory.InitWithDefaultValues().Build();
        var invalidContent = new string('a', length); // Exceeding max length

        // Act
        var newContentResult = TheString.Create(invalidContent);

        // Assert
        Assert.True(newContentResult.IsFailure);
        Assert.Contains(Error.TooLongString(TheString.MAX_LENGTH), newContentResult.Error.EnumerateAll());
    }

    // ID:UC7.F4
    [Fact]
    public void UpdateContentTweet_ContentTooLongWithSpecialCharacters_ReturnFailure() {
        // Arrange
        var post = PostFactory.InitWithDefaultValues().Build();
        var invalidContent = "a87bfD9E6afda16F!@$^1b9C0Cd%A1B8bF182Ee301Df30Fa0@$6Dd79e0ebdAa4D137Fc6e@#$%d42aE60f7420dE6D48b548dF8CE#%C35E230AdbFa#eF6C4fc92Da214F98a9E657c";

        // Act
        var newContentResult = TheString.Create(invalidContent);

        // Assert
        Assert.True(newContentResult.IsFailure);
        Assert.Contains(Error.TooLongString(TheString.MAX_LENGTH), newContentResult.Error.EnumerateAll());
    }
}