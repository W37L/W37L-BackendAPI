using UnitTests.Common.Factories;
using ViaEventAssociation.Core.Domain.Aggregates.Guests;

namespace UnitTests.Features.User.UpdateUserName;

public class UserUpdateFirstNameTests {
    //ID:UC3.S1
    [Theory]
    [InlineData("Alice")]
    [InlineData("Bob")]
    [InlineData("Charlie")]
    [InlineData("Diana")]
    [InlineData("Eve")]
    [InlineData("Frank")]
    public void UpdateFirstName_ValidFirstName_ReturnSuccess(string? newFirstName) {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();
        var newFirstNameType = NameType.Create(newFirstName).Payload;

        // Act
        var result = user.UpdateFirstName(newFirstNameType);

        // Assert
        Assert.True(result.IsSuccess);
    }

    //ID:UC3.F1
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void UpdateFirstName_InvalidFirstName_FirstNameBlank_ReturnFailure(string? invalidFirstName) {
        var user = UserFactory.InitWithDefaultValues().Build();
        var newFirstNameResult = NameType.Create(invalidFirstName);

        Assert.True(newFirstNameResult.IsFailure);
        Assert.Contains(Error.BlankOrNullString, newFirstNameResult.Error.EnumerateAll());
    }

    //ID:UC3.F2
    [Theory]
    [InlineData("A")]
    [InlineData("B")]
    [InlineData("C")]
    public void UpdateFirstName_InvalidFirstName_FirstNameTooShort_ReturnFailure(string? invalidFirstName) {
        var user = UserFactory.InitWithDefaultValues().Build();
        var newFirstNameResult = NameType.Create(invalidFirstName);

        Assert.True(newFirstNameResult.IsFailure);
        Assert.Contains(Error.TooShortName(NamingType.MIN_LENGTH), newFirstNameResult.Error.EnumerateAll());
    }

    //ID:UC3.F3
    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyzs")]
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYFD")]
    [InlineData("Zabcdefghijklmnopqrstuvwasdfghjklzxcvbnm")]
    public void UpdateFirstName_InvalidFirstName_FirstNameTooLong_ReturnFailure(string? invalidFirstName) {
        var user = UserFactory.InitWithDefaultValues().Build();
        var newFirstNameResult = NameType.Create(invalidFirstName);

        Assert.True(newFirstNameResult.IsFailure);
        Assert.Contains(Error.TooLongName(NamingType.MAX_LENGTH), newFirstNameResult.Error.EnumerateAll());
    }

    //ID:UC3.F4
    [Theory]
    [InlineData("John1")]
    [InlineData("Anna-")]
    [InlineData("Eve$")]
    public void UpdateFirstName_InvalidFirstName_ContainsInvalidCharacter_ReturnFailure(string? invalidFirstName) {
        var user = UserFactory.InitWithDefaultValues().Build();
        var newFirstNameResult = NameType.Create(invalidFirstName);

        Assert.True(newFirstNameResult.IsFailure);
        Assert.Contains(Error.InvalidName, newFirstNameResult.Error.EnumerateAll());
    }
}