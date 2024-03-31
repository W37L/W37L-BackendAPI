using UnitTests.Common.Factories;
using W3TL.Core.Domain.Agregates.User.Entity.Values;

public class UpdateBannerTest {
    // Success Scenarios

    // ID:UC2.S2
    [Theory]
    [InlineData("https://example.com/banner.jpg", "https://example.com/banner.jpg")]
    [InlineData("http://example.com/banner.png", "http://example.com/banner.png")]
    [InlineData("www.example.com/banner.gif", "https://www.example.com/banner.gif")]
    public void UpdateBanner_ValidUrl_ShouldNormalizeAndUpdateSuccessfully(string inputUrl, string expectedUrl) {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();

        // Act
        var bannerResult = BannerType.Create(inputUrl);
        Assert.True(bannerResult.IsSuccess, "Banner creation should succeed.");

        var result = user.Profile.UpdateBanner(bannerResult.Payload);

        // Assert
        Assert.True(result.IsSuccess);

        Assert.Equal(expectedUrl, user.Profile.Banner.Url);
    }

    // Failure Scenarios

    // ID:UC2.F7
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void UpdateBanner_InvalidUrl_BlankOrWhitespace_ShouldFail(string url) {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();

        // Act
        var result = BannerType.Create(url);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.BlankString, result.Error.EnumerateAll());
    }

    // ID:UC2.F8
    [Theory]
    [InlineData("justastring")]
    [InlineData("ftp://example.com/banner.jpg")]
    [InlineData("##__!!")]
    public void UpdateBanner_InvalidUrl_FormatError_ShouldFail(string url) {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();

        // Act
        var result = BannerType.Create(url);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.InvalidUrl, result.Error.EnumerateAll());
    }
}