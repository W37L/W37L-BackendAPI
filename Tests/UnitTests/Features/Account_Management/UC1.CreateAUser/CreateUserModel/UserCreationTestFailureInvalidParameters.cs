using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.User.Values;

public class UserCreationTestFailureInvalidParameters {
    [Fact]
    public void CreateUser_InvalidParameters_ReturnErrorList() {
        // Arrange
        var invalidFirstName = NameType.Create("1"); // Name with a number should fail
        var invalidLastName = LastNameType.Create(null); // Null last name should fail
        var invalidEmail = EmailType.Create("notanemail"); // Invalid email format should fail
        var invalidPub = PubType.Create(""); // Empty pub should fail

        // Act
        try {
            User.Create(null, invalidFirstName.Payload, invalidLastName.Payload, invalidEmail.Payload, invalidPub.Payload);
        }
        catch (Exception e) {
            Assert.IsType<ArgumentNullException>(e);
        }
    }
}