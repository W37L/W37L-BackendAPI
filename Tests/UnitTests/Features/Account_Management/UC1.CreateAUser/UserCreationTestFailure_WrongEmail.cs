using W3TL.Core.Domain.Agregates.User.Values;

namespace UnitTests.Features.User.CreateAUser;

public class UserCreationTestFailure_WrongEmail {
    // UC1.F17 - Test for failure when Email is blank
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateEmailType_BlankEmail_ReturnBlankStringError(string? emailInput) {
        // Arrange

        // Act
        var result = EmailType.Create(emailInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.BlankOrNullString, result.Error.EnumerateAll());
    }

    // UC1.F18 - Test for failure when Email is in incorrect format
    [Theory]
    [InlineData("emailwithoutat.com")]
    [InlineData("incomplete@")]
    [InlineData("@nouser.com")]
    public void CreateEmailType_InvalidFormatEmail_ReturnInvalidEmailError(string? emailInput) {
        // Arrange

        // Act
        var result = EmailType.Create(emailInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidEmail, result.Error.EnumerateAll());
    }

    // UC1.F19 - Test for failure when Email is missing domain part
    [Theory]
    [InlineData("nodomain@com")]
    [InlineData("missing@.")]
    [InlineData("user@missingdot")]
    public void CreateEmailType_MissingDomainEmail_ReturnInvalidEmailError(string? emailInput) {
        // Arrange

        // Act
        var result = EmailType.Create(emailInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidEmail, result.Error.EnumerateAll());
    }

    // UC1.F20 - Test for failure when Email is missing username part
    [Theory]
    [InlineData("@domain.com")]
    [InlineData("@example.com")]
    [InlineData("@test.com")]
    public void CreateEmailType_MissingUsernameEmail_ReturnInvalidEmailError(string? emailInput) {
        // Arrange

        // Act
        var result = EmailType.Create(emailInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidEmail, result.Error.EnumerateAll());
    }
}