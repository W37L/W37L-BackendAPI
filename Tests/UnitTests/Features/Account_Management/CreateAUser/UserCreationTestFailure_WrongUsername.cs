using Xunit;
using W3TL.Core.Domain.Agregates.User.Values;
using System.Linq;

namespace UnitTests.Features.User.CreateAUser;

public class UserCreationTestFailure_WrongUsername {

    // UC1.F5 - Test for failure when Username is blank
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateUser_BlankUsername_ReturnBlankUserNameError(string usernameInput) {
        // Arrange

        // Act
        var result = UserNameType.Create(usernameInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.BlankUserName, result.Error.EnumerateAll());
    }

    // UC1.F6 - Test for failure when Username contains special characters not allowed
    [Theory]
    [InlineData("invalid#username")]
    [InlineData("username@here")]
    [InlineData("username&123")]
    public void CreateUser_SpecialCharacterInUsername_ReturnInvalidUserNameFormatError(string usernameInput) {
        // Arrange

        // Act
        var result = UserNameType.Create(usernameInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidUserNameFormat, result.Error.EnumerateAll());
    }

    // UC1.F7 - Test for failure when Username is too short
    // Assuming the minimum valid length is 2 characters based on the initial configuration
    [Theory]
    [InlineData("a")] // Too short
    [InlineData("1")] // Too short
    [InlineData("_")] // Too short
    public void CreateUser_TooShortUsername_ReturnInvalidUserNameFormatError(string usernameInput) {
        // Arrange

        // Act
        var result = UserNameType.Create(usernameInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidUserNameFormat, result.Error.EnumerateAll());
    }

    // UC1.F8 - Test for failure when Username exceeds the length limit at various lengths
    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdef")] // 32 characters
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefg1234567")] // 39 characters
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefg1234567890")] // 44 characters
    public void CreateUser_TooLongUsername_ReturnInvalidUserNameFormatError(string usernameInput) {
        // Arrange

        // Act
        var result = UserNameType.Create(usernameInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidUserNameFormat, result.Error.EnumerateAll());
    }

    // Additional tests can be added here for other failure scenarios as necessary
}
