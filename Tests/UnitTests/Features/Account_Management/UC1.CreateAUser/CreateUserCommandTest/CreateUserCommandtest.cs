using UnitTests.Common.Factories;
using W3TL.Core.Application.CommandDispatching.Commands.User;

namespace UnitTests.Features.Account_Management.UC1.CreateAUser.CreateUserCommandTest;

public class CreateUserCommandtest {
    // Test to check if the user is created successfully
    //ID:UC1.S1
    [Fact]
    public void Create_ValidInput_Success() {
        // Arrange
        var userId = ValidFields.VALID_USER_ID;
        var userName = ValidFields.VALID_USERNAME;
        var firstName = ValidFields.VALID_FIRST_NAME;
        var lastName = ValidFields.VALID_LAST_NAME;
        var email = ValidFields.VALID_EMAIL;
        var pubKey = ValidFields.VALID_PUB_KEY;

        // Act
        var result = CreateUserCommand.Create(userId!, userName!, firstName!, lastName!, email!, pubKey!);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(userId, result.Payload.UserId.Value);
        Assert.Equal(userName, result.Payload.UserName.Value);
        Assert.Equal(firstName, result.Payload.FirstName.Value);
        Assert.Equal(lastName, result.Payload.LastName.Value);
        Assert.Equal(email, result.Payload.Email.Value);
        Assert.Equal(pubKey, result.Payload.Pub.Value);
    }

    // Test to check if the user is not created successfully
    //ID:UC1.F1
    [Fact]
    public void Create_InvalidInput_Failure() {
        // Arrange
        var userId = "Invalid";
        var userName = "";
        var firstName = "";
        var lastName = "";
        var email = "Invalid";
        var pubKey = "Invalid";


        // Act
        var result = CreateUserCommand.Create(userId, userName, firstName, lastName, email, pubKey);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidEmail, result.Error.EnumerateAll());
        Assert.Contains(Error.BlankUserName, result.Error.EnumerateAll());
        Assert.Contains(Error.InvalidLength, result.Error.EnumerateAll());
    }
}