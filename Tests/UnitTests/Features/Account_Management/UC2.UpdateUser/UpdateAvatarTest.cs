using UnitTests.Common.Factories;
using W3TL.Core.Domain.Agregates.User.Entity.Values;

namespace UnitTests.Features.Account_Management.UC2.UpdateUser;

public class UpdateAvatarTest {
    // Success Scenario

    // ID:UC2.S2
    [Theory]
    [InlineData("https://example.com/avatar.jpg", "https://example.com/avatar.jpg")] // Already valid https URL
    [InlineData("http://example.com/avatar.png", "http://example.com/avatar.png")] // Already valid http URL
    [InlineData("www.example.com/avatar.gif", "https://www.example.com/avatar.gif")] // www URL gets https prepended
    [InlineData("example.com/avatar", "example.com/avatar")] // Implicit https prepending for no protocol
    public void UpdateAvatar_ValidUrl_ShouldNormalizeAndUpdateSuccessfully(string? inputUrl, string expectedUrl) {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();

        // Act
        var avatarResult = AvatarType.Create(inputUrl); // Assume this returns a Result<AvatarType>
        Assert.True(avatarResult.IsSuccess); // Ensure AvatarType creation succeeds

        var result = user.Profile.UpdateAvatar(avatarResult.Payload);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedUrl, user.Profile.Avatar.Url);
    }

    // Failure Scenarios

    // ID:UC2.F7
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void UpdateAvatar_InvalidUrl_BlankOrWhitespace_ShouldFail(string? url) {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();

        // Act
        var result = AvatarType.Create(url);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.BlankOrNullString, result.Error.EnumerateAll());
    }

    // ID:UC2.F8
    [Theory]
    [InlineData("justastring")]
    [InlineData("ftp://example.com/avatar.jpg")]
    [InlineData("##__!!")]
    public void UpdateAvatar_InvalidUrl_FormatError_ShouldFail(string? url) {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();

        // Act
        var result = AvatarType.Create(url);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.InvalidUrl, result.Error.EnumerateAll());
    }
}