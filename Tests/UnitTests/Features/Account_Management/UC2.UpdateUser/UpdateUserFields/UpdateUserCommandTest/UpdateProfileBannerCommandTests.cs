using UnitTests.Common.Factories;
using W3TL.Core.Application.CommandDispatching.Commands.User;

public class UpdateProfileBannerCommandTests {
    [Fact]
    public void Create_ValidInput_Success() {
        // Arrange
        var userId = ValidFields.VALID_USER_ID;
        var bannerUrl = ValidFields.VALID_BANNER_URL;

        // Act
        var result = UpdateProfileBannerCommand.Create(userId, bannerUrl);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(bannerUrl, result.Payload.Banner.Url);
    }

    [Fact]
    public void Create_InvalidInput_Failure() {
        // Arrange
        var userId = "InvalidUserId"; // Assuming UserID creation will fail
        var bannerUrl = ""; // Invalid because it's empty

        // Act
        var result = UpdateProfileBannerCommand.Create(userId, bannerUrl);

        // Assert
        Assert.True(result.IsFailure);
        // Since multiple errors could be added, check if at least one is present
        Assert.NotEmpty(result.Error.EnumerateAll());
    }
}