using UnitTests.Common.Factories;
using W3TL.Core.Domain.Agregates.User.Values;

namespace UnitTests.Features.User.UpdateUserName;

public class UserUpdateUserNameTestsSuccess {
    //ID:UC2.S1
    [Theory]
    [InlineData("ValidUsername")]
    [InlineData("Username_2")]
    [InlineData("123_username")]
    [InlineData("user.name")]
    [InlineData("name1234")]
    [InlineData("a_user_name")]
    public void UpdateUserName_ValidUsername_ReturnSuccess(string? newUsername) {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();
        var newUserNameType = UserNameType.Create(newUsername).Payload;

        // Act
        var result = user.UpdateUserName(newUserNameType);

        // Assert
        Assert.True(result.IsSuccess);
    }

    //ID:UC2.F2
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void UpdateUserName_InvalidUsername_UserNameBlank_ReturnFailure(string? invalidUsername) {
        var user = UserFactory.InitWithDefaultValues().Build();
        var newUserNameResult = UserNameType.Create(invalidUsername);

        Assert.True(newUserNameResult.IsFailure);
        Assert.Contains(Error.BlankUserName, newUserNameResult.Error.EnumerateAll());
    }

    //ID:UC2.F3
    [Theory]
    [InlineData("a")]
    [InlineData("b")]
    [InlineData("1")]
    public void UpdateUserName_InvalidUsername_UserNameTooShort_ReturnFailure(string? invalidUsername) {
        var user = UserFactory.InitWithDefaultValues().Build();
        var newUserNameResult = UserNameType.Create(invalidUsername);

        Assert.True(newUserNameResult.IsFailure);
        Assert.Contains(Error.InvalidUserNameFormat, newUserNameResult.Error.EnumerateAll());
    }

    //ID:UC2.F4
    [Theory]
    [InlineData("_username")]
    [InlineData(".username")]
    [InlineData("_1a")]
    public void UpdateUserName_InvalidUsername_StartsWithInvalidCharacter_ReturnFailure(string? invalidUsername) {
        var user = UserFactory.InitWithDefaultValues().Build();
        var newUserNameResult = UserNameType.Create(invalidUsername);

        Assert.True(newUserNameResult.IsFailure);
        Assert.Contains(Error.InvalidUserNameFormat, newUserNameResult.Error.EnumerateAll());
    }

    //ID:UC2.F5
    [Theory]
    [InlineData("inva!d")]
    [InlineData("invalid#username")]
    [InlineData("no$allowed")]
    public void UpdateUserName_InvalidUsername_ContainsInvalidCharacter_ReturnFailure(string? invalidUsername) {
        var user = UserFactory.InitWithDefaultValues().Build();
        var newUserNameResult = UserNameType.Create(invalidUsername);

        Assert.True(newUserNameResult.IsFailure);
        Assert.Contains(Error.InvalidUserNameFormat, newUserNameResult.Error.EnumerateAll());
    }

    //ID:UC2.F6
    [Theory]
    [InlineData("thisiswaytoolongofausernameforthis")]
    [InlineData("usernameistoolongbeyondthelimitset")]
    [InlineData("exceedingthefifteencharlimit")]
    public void UpdateUserName_InvalidUsername_UserNameTooLong_ReturnFailure(string? invalidUsername) {
        var user = UserFactory.InitWithDefaultValues().Build();
        var newUserNameResult = UserNameType.Create(invalidUsername);

        Assert.True(newUserNameResult.IsFailure);
        Assert.Contains(Error.InvalidUserNameFormat, newUserNameResult.Error.EnumerateAll());
    }
}