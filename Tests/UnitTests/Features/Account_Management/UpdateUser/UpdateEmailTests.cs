
using UnitTests.Common.Factories;
using W3TL.Core.Domain.Agregates.User.Values;

public class UserUpdateEmailTests {

    // Expanded Success Scenarios

    //ID:UC5.S1
    [Theory]
    [InlineData("email@example.com")]
    [InlineData("firstname.lastname@example.com")]
    [InlineData("email@subdomain.example.com")]
    [InlineData("firstname+lastname@example.com")]
    [InlineData("1234567890@example.com")]
    [InlineData("email@domain.co.uk")]
    [InlineData("email@domain.ar")]
    [InlineData("email@domain.io")]
    public void UpdateEmail_ValidEmail_ReturnSuccess(string newEmail) {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();
        var newEmailType = EmailType.Create(newEmail).Value;

        // Act
        var result = user.UpdateEmail(newEmailType);

        // Assert
        Assert.True(result.IsSuccess);
    }

    // Expanded Failure Scenarios

    //ID:UC5.F2
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void UpdateEmail_InvalidEmail_EmailBlank_ReturnFailure(string invalidEmail) {
        var user = UserFactory.InitWithDefaultValues().Build();
        var newEmailResult = EmailType.Create(invalidEmail);

        Assert.True(newEmailResult.IsFailure);
        Assert.Contains(Error.BlankString, newEmailResult.Error.EnumerateAll());
    }

    //ID:UC5.F3
    [Theory]
    [InlineData("plainaddress")]
    [InlineData("@missingusername.com")]
    [InlineData("Joe Smith <email@example.com>")]
    [InlineData("email@example")]
    [InlineData("email@-example.com")]
    [InlineData("email@example..com")]
    public void UpdateEmail_InvalidEmail_FormatError_ReturnFailure(string invalidEmail) {
        var user = UserFactory.InitWithDefaultValues().Build();
        var newEmailResult = EmailType.Create(invalidEmail);

        Assert.True(newEmailResult.IsFailure);
        Assert.Contains(Error.InvalidEmail, newEmailResult.Error.EnumerateAll());
    }

    //ID:UC5.F4
    [Theory]
    [InlineData("email.example.com")]
    [InlineData("email@example@example.com")]
    [InlineData("email@example.com (Joe Smith)")]
    [InlineData("email@example.com.")]
    [InlineData(".email@example.com")]
    [InlineData("email@.example.com")]
    [InlineData("email..email@example.com")]
    [InlineData("email@example_com")]
    public void UpdateEmail_InvalidEmail_MultipleAtsOrMissingDot_ReturnFailure(string invalidEmail) {
        var user = UserFactory.InitWithDefaultValues().Build();
        var newEmailResult = EmailType.Create(invalidEmail);

        Assert.True(newEmailResult.IsFailure);
        Assert.Contains(Error.InvalidEmail, newEmailResult.Error.EnumerateAll());
    }

    //ID:UC5.F5
    [Theory]
    [InlineData("email@111.222.333.44444")]
    [InlineData("email@example..com")]
    [InlineData("email@.com")]
    [InlineData("email@-example.com")]
    public void UpdateEmail_InvalidEmail_InvalidCharactersOrPatterns_ReturnFailure(string invalidEmail) {
        var user = UserFactory.InitWithDefaultValues().Build();
        var newEmailResult = EmailType.Create(invalidEmail);

        Assert.True(newEmailResult.IsFailure);
        Assert.Contains(Error.InvalidEmail, newEmailResult.Error.EnumerateAll());
    }
}
