using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.User.Values;
using W3TL.Core.Domain.Common.Bases;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Domain.Agregates.User;

public class User : AggregateRoot<UserID> {


    public UserNameType UserName { get; }
    public NameType FirstName { get; }
    public LastNameType LastName { get; }
    public EmailType Email { get; }
    public PubType Pub { get; }
    public CreatedAtType CreatedAt { get; }


    private User(
        UserID userId,
        UserNameType userName,
        NameType firstName,
        LastNameType lastName,
        EmailType email,
        PubType pub,
        CreatedAtType createdAt
    ) : base(userId) {
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Pub = pub;
        CreatedAt = createdAt;
    }

    public static Result<User> Create(
        UserID userId,
        UserNameType userName,
        NameType firstName,
        LastNameType lastName,
        EmailType email,
        PubType pub,
        CreatedAtType createdAt
    ) {
        try {
            var user = new User(userId, userName, firstName, lastName, email, pub, createdAt);
            return user;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}