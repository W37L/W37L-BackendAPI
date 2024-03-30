using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.User.Values;
using W3TL.Core.Domain.Common.Values;
using static UnitTests.Common.Factories.ValidFields;

namespace UnitTests.Common.Factories;

public class UserFactory {
    private User _user { get; set; }

    private UserFactory() { }

    public static UserFactory Init() {
        var factory = new UserFactory();
        var userId = UserID.Generate().Value;
        factory._user = new User(userId);
        return new UserFactory();
    }

    public static UserFactory InitWithDefaultValues() {
        var factory = new UserFactory();
        var userName = UserNameType.Create(VALID_USERNAME).Value;
        var firstName = NameType.Create(VALID_FIRST_NAME).Value;
        var lastName = LastNameType.Create(VALID_LAST_NAME).Value;
        var email = EmailType.Create(VALID_EMAIL).Value;
        var pub = PubType.Create(VALID_PUB_KEY).Value;

        var result = User.Create(userName, firstName, lastName, email, pub);

        factory._user = result.Value;
        return factory;
        }

    public User Build() {
        return _user;
    }

    public UserFactory WitValidId(string uid) {
        _user.Id = UserID.Create(uid).Value;
        return this;
    }

    public UserFactory WithValidUserName(string userName) {
        _user.UserName = UserNameType.Create(userName).Value;
        return this;
    }

    public UserFactory WithValidFirstName(string firstName) {
        _user.FirstName = NameType.Create(firstName).Value;
        return this;
    }

    public UserFactory WithValidLastName(string lastName) {
        _user.LastName = LastNameType.Create(lastName).Value;
        return this;
    }

    public UserFactory WithValidEmail(string email) {
        _user.Email = EmailType.Create(email).Value;
        return this;
    }

    public UserFactory WithValidPubKey(string pubKey) {
        _user.Pub = PubType.Create(pubKey).Value;
        return this;
    }
}