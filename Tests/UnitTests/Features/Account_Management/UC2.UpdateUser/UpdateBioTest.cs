using UnitTests.Common.Factories;
using W3TL.Core.Domain.Agregates.User.Entity.Values;

namespace UnitTests.Features.Account_Management.UC2.UpdateUser;

public class UpdateBioTest {
    // Success Scenarios
    [Theory]
    [InlineData("Valid bio within the character limit.")] // Short bio
    [InlineData("This bio exactly hits the 500 character limit, filled to the brim but just fitting within the constraints set by the system. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.")] // Max length bio
    [InlineData("Bio with\nbreaks and\ttabs.")] // Bio with special characters
    public void UpdateBio_ValidInput_ShouldUpdateSuccessfully(string bioText) {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();


        // Act
        var bioResult = BioType.Create(bioText);


        var result = user.Profile.UpdateBio(bioResult.Payload);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(bioText, user.Profile.Bio?.Value);
    }

    // Failure Scenarios
    [Theory]
    [InlineData(null)] // Null bio
    public void UpdateBio_InvalidBio_ShouldFail(string bioText) {
        // Arrange

        // Act
        var result = BioType.Create(bioText);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.BlankString, result.Error.EnumerateAll());
    }

    // Bio exceeding maximum length
    [Theory]
    [InlineData(501)] // Just over limit
    [InlineData(502)] // Slightly over limit
    [InlineData(600)] // Moderately over limit
    [InlineData(1001)] // Significantly over limit
    public void UpdateBio_TooLongBio_ShouldFail(int charCount) {
        // Arrange
        var bioText = new string('a', charCount);

        // Act
        var result = BioType.Create(bioText);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.TooLongBio(BioType.MAX_LENGTH), result.Error.EnumerateAll());
    }
}