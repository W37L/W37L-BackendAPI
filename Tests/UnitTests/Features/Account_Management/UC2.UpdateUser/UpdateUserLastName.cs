using UnitTests.Common.Factories;
using ViaEventAssociation.Core.Domain.Aggregates.Guests;

namespace UnitTests.Features.User.UpdateUserName;

public class UpdateUserLastName {
    // Success Scenarios

    //ID:UC4.S1
    [Theory]
    [InlineData("Smith")]
    [InlineData("Johnson")]
    [InlineData("Williams")]
    [InlineData("Brown")]
    [InlineData("Jones")]
    [InlineData("Garcia")]
    public void UpdateLastName_ValidLastName_ReturnSuccess(string? newLastName) {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();
        var newLastNameType = LastNameType.Create(newLastName).Payload;

        // Act
        var result = user.UpdateLastName(newLastNameType);

        // Assert
        Assert.True(result.IsSuccess);
    }

    // Failure Scenarios

    //ID:UC4.F2
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void UpdateLastName_InvalidLastName_LastNameBlank_ReturnFailure(string? invalidLastName) {
        var user = UserFactory.InitWithDefaultValues().Build();
        var newLastNameResult = LastNameType.Create(invalidLastName);

        Assert.True(newLastNameResult.IsFailure);
        Assert.Contains(Error.BlankOrNullString, newLastNameResult.Error.EnumerateAll());
    }

    //ID:UC4.F3
    [Theory]
    [InlineData("A")]
    [InlineData("B")]
    [InlineData("C")]
    public void UpdateLastName_InvalidLastName_LastNameTooShort_ReturnFailure(string? invalidLastName) {
        var user = UserFactory.InitWithDefaultValues().Build();
        var newLastNameResult = LastNameType.Create(invalidLastName);

        Assert.True(newLastNameResult.IsFailure);
        Assert.Contains(Error.TooShortName(NamingType.MIN_LENGTH), newLastNameResult.Error.EnumerateAll());
    }

    //ID:UC4.F4
    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyz")]
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
    [InlineData("Zyxwvutsrqponmlkjihgfedcba")]
    public void UpdateLastName_InvalidLastName_LastNameTooLong_ReturnFailure(string? invalidLastName) {
        var user = UserFactory.InitWithDefaultValues().Build();
        var newLastNameResult = LastNameType.Create(invalidLastName);

        Assert.True(newLastNameResult.IsFailure);
        Assert.Contains(Error.TooLongName(NamingType.MAX_LENGTH), newLastNameResult.Error.EnumerateAll());
    }

    //ID:UC4.F5
    [Theory]
    [InlineData("Smith1")]
    [InlineData("O'Neil")]
    [InlineData("Johnson$")]
    public void UpdateLastName_InvalidLastName_ContainsInvalidCharacter_ReturnFailure(string? invalidLastName) {
        var user = UserFactory.InitWithDefaultValues().Build();
        var newLastNameResult = LastNameType.Create(invalidLastName);

        Assert.True(newLastNameResult.IsFailure);
        Assert.Contains(Error.InvalidName, newLastNameResult.Error.EnumerateAll());
    }
}