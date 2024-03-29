
using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.User.Entity;
using W3TL.Core.Domain.Agregates.User.Values;
using W3TL.Core.Domain.Common.Bases;
using W3TL.Core.Domain.Common.Values;

public class User : AggregateRoot<UserID> {

    public UserNameType UserName { get; }
    public NameType FirstName { get; }
    public LastNameType LastName { get; }
    public EmailType Email { get; }
    public PubType Pub { get; }
    public CreatedAtType CreatedAt { get; }
    public Profile Profile { get; private set; }

    public List<User> Followers { get; private set; }
    public List<User> Following { get; private set; }
    public List<User> Blocked { get; private set; }
    public List<User> Muted { get; private set; }

    public List<Post> Posts { get; private set; }


    private User(
        UserID userId,
        UserNameType userName,
        NameType firstName,
        LastNameType lastName,
        EmailType email,
        PubType pub,
        CreatedAtType createdAt,
        Profile profile
    ) : base(userId) {
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Pub = pub;
        CreatedAt = createdAt;
        Profile = profile;
    }

    public static Result<User> Create(
        UserID userId,
        UserNameType userName,
        NameType firstName,
        LastNameType lastName,
        EmailType email,
        PubType pub,
        CreatedAtType createdAt,
        Profile profile
    ) {
        try {
            var user = new User(userId, userName, firstName, lastName, email, pub, createdAt, profile);
            return user;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public static Result<User> Create(UserNameType userName, NameType firstName, LastNameType lastName, EmailType email, PubType pub) {
        try {
            var userId = UserID.Generate().Value;
            var profile = Profile.Create(userId).Value;
            var createdAt = CreatedAtType.Create().Value;
            var user = new User(userId, userName, firstName, lastName, email, pub, createdAt, profile);
            return user;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}