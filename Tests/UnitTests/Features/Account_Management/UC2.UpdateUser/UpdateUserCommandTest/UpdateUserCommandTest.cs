using UnitTests.Common.Factories;
using W3TL.Core.Application.CommandDispatching.Commands.User;

namespace UnitTests.Features.Account_Management.UC2.UpdateUser.UpdateUserCommandTest;

public class UpdateUserCommandTest {
    //ID.UC2.S1
    [Fact]
    public void Update_ValidInput_Success() {
        // Arrange
        var userId = ValidFields.VALID_USER_ID;
        var userName = ValidFields.VALID_USERNAME;
        var firstName = ValidFields.VALID_FIRST_NAME;
        var lastName = ValidFields.VALID_LAST_NAME;
        var bio = ValidFields.VALID_BIO;
        var location = ValidFields.VALID_LOCATION;
        var website = ValidFields.VALID_WEBSITE;


        // Act
        var result = UpdateUserCommand.Create(userId!, userName!, firstName!, lastName!, bio!, location!, website!);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(userId, result.Payload.Id.Value);
        Assert.Equal(userName, result.Payload.UserName.Value);
        Assert.Equal(firstName, result.Payload.FirstName.Value);
        Assert.Equal(lastName, result.Payload.LastName.Value);
        Assert.Equal(bio, result.Payload.Bio.Value);
        Assert.Equal(location, result.Payload.Location.Value);
        Assert.Equal(website, result.Payload.Website.Url);
    }

    //ID.UC2.F1
    [Fact]
    public void Update_InvalidCountArgs_Failure() {
        // Arrange
        var userId = ValidFields.VALID_USER_ID;
        var userName = ValidFields.VALID_USERNAME;
        var firstName = ValidFields.VALID_FIRST_NAME;
        var lastName = ValidFields.VALID_LAST_NAME;
        var bio = ValidFields.VALID_BIO;
        var location = ValidFields.VALID_LOCATION;
        var website = ValidFields.VALID_WEBSITE;

        // Act
        var result = UpdateUserCommand.Create(userId!, userName!, firstName!, lastName!, bio!, location!);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidCommand, result.Error.EnumerateAll());
    }
}