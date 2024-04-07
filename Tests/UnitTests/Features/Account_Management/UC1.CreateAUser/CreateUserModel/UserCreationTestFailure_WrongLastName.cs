using ViaEventAssociation.Core.Domain.Aggregates.Guests;

namespace UnitTests.Features.User.CreateAUser;

public class UserCreationTestFailureWrongLastName {
    // UC1.F13 - Test for failure when LastName is blank
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateLastNameType_BlankLastName_ReturnBlankStringError(string? lastNameInput) {
        // Arrange

        // Act
        var result = LastNameType.Create(lastNameInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.BlankOrNullString, result.Error.EnumerateAll());
    }

    // UC1.F14 - Test for failure when LastName contains characters not allowed
    [Theory]
    [InlineData("Doe!")]
    [InlineData("O'Neil@")]
    [InlineData("Smith#123")] // Contains a digit and special character, not allowed
    public void CreateLastNameType_InvalidCharacterInLastName_ReturnInvalidNameError(string? lastNameInput) {
        // Arrange

        // Act
        var result = LastNameType.Create(lastNameInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidName, result.Error.EnumerateAll());
    }

    // UC1.F15 - Test for failure when LastName is too short
    [Theory]
    [InlineData("D")]
    [InlineData("O")]
    [InlineData("S")]
    public void CreateLastNameType_TooShortLastName_ReturnTooShortNameError(string? lastNameInput) {
        // Arrange

        // Act
        var result = LastNameType.Create(lastNameInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.TooShortName(NamingType.MIN_LENGTH), result.Error.EnumerateAll());
    }

    // UC1.F16 - Test for failure when LastName exceeds the length limit
    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdef")]
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefg1234567")]
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefg1234567890")]
    public void CreateLastNameType_TooLongLastName_ReturnTooLongNameError(string? lastNameInput) {
        // Arrange

        // Act
        var result = LastNameType.Create(lastNameInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.TooLongName(NamingType.MAX_LENGTH), result.Error.EnumerateAll());
    }
}