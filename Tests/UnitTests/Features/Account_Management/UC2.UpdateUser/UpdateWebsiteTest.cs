using UnitTests.Common.Factories;
using W3TL.Core.Domain.Agregates.User.Entity.Values;

namespace UnitTests.Features.Account_Management.UC2.UpdateUser;

public class UpdateWebsiteTest {
    // Success Scenarios

    // ID:UC2.S3
    [Theory]
    [InlineData("http://example.com", "http://example.com")]
    [InlineData("https://example.com", "https://example.com")]
    [InlineData("www.example.com", "https://www.example.com")]
    public void UpdateWebsite_ValidUrl_ShouldNormalizeAndUpdateSuccessfully(string inputUrl, string expectedUrl) {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();

        // Act
        var websiteResult = WebsiteType.Create(inputUrl);
        Assert.True(websiteResult.IsSuccess, "Website creation should succeed.");

        var result = user.Profile.UpdateWebsite(websiteResult.Payload);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedUrl, user.Profile.Website.Url);
    }

    // Failure Scenarios

    // ID:UC2.F10
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void UpdateWebsite_InvalidUrl_BlankOrWhitespace_ShouldFail(string url) {
        // Arrange

        // Act
        var result = WebsiteType.Create(url);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.BlankString, result.Error.EnumerateAll());
    }

    // ID:UC2.F11
    [Theory]
    [InlineData("justastring")]
    [InlineData("ftp://example.com")]
    [InlineData("##__!!")]
    public void UpdateWebsite_InvalidUrl_FormatError_ShouldFail(string url) {
        // Arrange

        // Act
        var result = WebsiteType.Create(url);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.InvalidUrl, result.Error.EnumerateAll());
    }
}