using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.User.Values;
using W3TL.Core.Domain.Common.Values;
using static UnitTests.Common.Factories.ValidFields;

namespace UnitTests.Common.Factories;

public class UserFactory {
    private UserFactory() { }
    private User _user { get; set; }

    public static UserFactory Init() {
        var factory = new UserFactory();
        var userId = UserID.Generate().Payload;
        factory._user = new User(userId);
        return factory;
    }

    public static UserFactory InitWithDefaultValues() {
        var factory = new UserFactory();
        var userId = UserID.Create(VALID_USER_ID).Payload;
        var userName = UserNameType.Create(VALID_USERNAME).Payload;
        var firstName = NameType.Create(VALID_FIRST_NAME).Payload;
        var lastName = LastNameType.Create(VALID_LAST_NAME).Payload;
        var email = EmailType.Create(VALID_EMAIL).Payload;
        var pub = PubType.Create(VALID_PUB_KEY).Payload;

        var result = User.Create(userId, userName, firstName, lastName, email, pub);

        factory._user = result.Payload;
        return factory;
    }

    public User Build() {
        return _user;
    }

    public UserFactory WitValidId(string? uid) {
        _user.Id = UserID.Create(uid).Payload;
        return this;
    }

    public UserFactory WithValidUserName(string? userName) {
        _user.UserName = UserNameType.Create(userName).Payload;
        return this;
    }

    public UserFactory WithValidFirstName(string? firstName) {
        _user.FirstName = NameType.Create(firstName).Payload;
        return this;
    }

    public UserFactory WithValidLastName(string? lastName) {
        _user.LastName = LastNameType.Create(lastName).Payload;
        return this;
    }

    public UserFactory WithValidEmail(string? email) {
        _user.Email = EmailType.Create(email).Payload;
        return this;
    }

    public UserFactory WithValidPubKey(string? pubKey) {
        _user.Pub = PubType.Create(pubKey).Payload;
        return this;
    }
}