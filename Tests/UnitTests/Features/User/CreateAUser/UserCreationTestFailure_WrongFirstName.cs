
using ViaEventAssociation.Core.Domain.Aggregates.Guests;

namespace UnitTests.Features.User.CreateAUser;

public class UserCreationTestFailure_WrongFirstName {

    // UC1.F9 - Test for failure when FirstName is blank
    [Theory]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData(null)]
    public void CreateNameType_BlankFirstName_ReturnBlankStringError(string firstNameInput) {
        // Arrange

        // Act
        var result = NameType.Create(firstNameInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.BlankString, result.Error.EnumerateAll());
    }

    // UC1.F10 - Test for failure when FirstName contains characters not allowed
    [Theory]
    [InlineData("John#")]
    [InlineData("An@na")]
    [InlineData("B0b")] // Contains a digit, which is not allowed
    public void CreateNameType_InvalidCharacterInFirstName_ReturnInvalidNameError(string firstNameInput) {
        // Arrange

        // Act
        var result = NameType.Create(firstNameInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidName, result.Error.EnumerateAll());
    }

    // UC1.F11 - Test for failure when FirstName is too short
    [Theory]
    [InlineData("J")] // One character
    [InlineData("A")]
    [InlineData("B")]
    public void CreateNameType_TooShortFirstName_ReturnTooShortNameError(string firstNameInput) {
        // Arrange

        // Act
        var result = NameType.Create(firstNameInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.TooShortName(NamingType.MIN_LENGTH), result.Error.EnumerateAll()); // Assuming MIN_LENGTH is accessible
    }

    // UC1.F12 - Test for failure when FirstName exceeds the length limit
    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyza")]
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz")]
    [InlineData("abcdefghijklmnopqrstuvwxyzcdefghijklmnopqrstuvwxyzabcdef")]
    public void CreateNameType_TooLongFirstName_ReturnTooLongNameError(string firstNameInput) {
        // Arrange

        // Act
        var result = NameType.Create(firstNameInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.TooLongName(NamingType.MAX_LENGTH), result.Error.EnumerateAll()); // Assuming MAX_LENGTH is accessible
    }
}
