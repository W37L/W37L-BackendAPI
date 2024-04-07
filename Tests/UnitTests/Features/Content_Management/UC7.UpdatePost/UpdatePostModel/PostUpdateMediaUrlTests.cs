using UnitTests.Common.Factories;
using W3TL.Core.Domain.Agregates.Post.Values;

namespace UnitTests.Features.Content_Management.UpdatePost;

public class PostUpdateMediaUrlTests {
    // Expanded Success Scenarios

    // ID:UC8.S1
    [Theory]
    [InlineData("https://example.com/media.png")]
    [InlineData("http://example.com/media.jpg")]
    [InlineData("www.example.com/media.gif")] // Assumes automatic "https://" prepend
    public void UpdateMediaUrl_ValidUrl_ReturnSuccess(string validUrl) {
        // Arrange
        var post = PostFactory.InitWithDefaultValues().Build();
        var newMediaUrl = MediaUrl.Create(validUrl).Payload;

        // Act
        var result = post.UpdateMediaUrl(newMediaUrl);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validUrl.StartsWith("www.") ? "https://" + validUrl : validUrl, post.MediaUrl.Url);
    }

    // Expanded Failure Scenarios

    // ID:UC8.F1
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void UpdateMediaUrl_BlankOrNullUrl_ReturnFailure(string invalidUrl) {
        // Arrange
        var post = PostFactory.InitWithDefaultValues().Build();

        // Act
        var result = MediaUrl.Create(invalidUrl);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.BlankOrNullString, result.Error.EnumerateAll());
    }

    // ID:UC8.F2
    [Theory]
    [InlineData("justastring")]
    [InlineData("ftp://example.com/media.png")]
    [InlineData("example dot com without protocol")]
    public void UpdateMediaUrl_InvalidUrlFormat_ReturnFailure(string invalidUrl) {
        // Arrange
        var post = PostFactory.InitWithDefaultValues().Build();

        // Act
        var result = MediaUrl.Create(invalidUrl);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidUrl, result.Error.EnumerateAll());
    }

    // ID:UC8.F3
    [Fact]
    public void UpdateMediaUrl_UrlWithInvalidCharacters_ReturnFailure() {
        // Arrange
        var invalidUrl = "https://@@$%^#3$example.com/<<media>>.png";

        // Act
        var result = MediaUrl.Create(invalidUrl);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidUrl, result.Error.EnumerateAll());
    }

    // ID:UC8.F4
    [Theory]
    [InlineData("https:// example.com/media.png")] // Space in URL
    [InlineData("https:///example.com/media.png")] // Triple slashes
    public void UpdateMediaUrl_UrlWithFormattingErrors_ReturnFailure(string invalidUrl) {
        // Arrange
        var post = PostFactory.InitWithDefaultValues().Build();

        // Act
        var result = MediaUrl.Create(invalidUrl);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidUrl, result.Error.EnumerateAll());
    }
}