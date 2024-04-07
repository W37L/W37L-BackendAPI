using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.User.Values;

public class UserCreationTestFailureInvalidParameters {
    [Fact]
    public void CreateUser_InvalidParameters_ReturnErrorList() {
        // Arrange
        var invalidUserName = UserNameType.Create(""); // Empty username should fail
        var invalidFirstName = NameType.Create("1"); // Name with a number should fail
        var invalidLastName = LastNameType.Create(null); // Null last name should fail
        var invalidEmail = EmailType.Create("notanemail"); // Invalid email format should fail
        var invalidPub = PubType.Create(""); // Empty pub should fail

        // Act
        var result = User.Create(invalidUserName.Payload, invalidFirstName.Payload, invalidLastName.Payload, invalidEmail.Payload, invalidPub.Payload);

        // Assert
        Assert.True(result.IsFailure, "User creation should fail due to invalid parameters.");
        var errors = result.Error.EnumerateAll().ToList();
        Assert.Contains(Error.BlankUserName, errors); // Expected due to empty username
        Assert.Contains(Error.InvalidName, errors); // Expected due to name with a number
        Assert.Contains(Error.InvalidEmail, errors); // Expected due to invalid email format
        Assert.Contains(Error.InvalidPublicKeyFormat, errors); // Expected due to empty pub
    }
}