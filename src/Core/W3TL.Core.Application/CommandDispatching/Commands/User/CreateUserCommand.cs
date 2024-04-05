using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.User.Values;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

public class CreateUserCommand : Command<UserID> {
    private CreateUserCommand(UserID userId, UserNameType userName, NameType firstName, LastNameType lastName, EmailType email, PubType pub) : base(userId) {
        UserId = userId;
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Pub = pub;
    }

    public UserID UserId { get; }
    public UserNameType UserName { get; }
    public NameType FirstName { get; }
    public LastNameType LastName { get; }
    public EmailType Email { get; }
    public PubType Pub { get; }

    public static Result<CreateUserCommand> Create(string uid, string userName, string firstName, string lastName, string email, string pub) {
        var errors = new HashSet<Error>();

        var Id = UserID.Create(uid)
            .OnFailure(error => errors.Add(error));

        var UserName = UserNameType.Create(userName)
            .OnFailure(error => errors.Add(error));

        var FirstName = NameType.Create(firstName)
            .OnFailure(error => errors.Add(error));

        var LastName = LastNameType.Create(lastName)
            .OnFailure(error => errors.Add(error));

        var Email = EmailType.Create(email)
            .OnFailure(error => errors.Add(error));

        var Pub = PubType.Create(pub)
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.CompileErrors(errors);

        return new CreateUserCommand(Id.Payload, UserName.Payload, FirstName.Payload, LastName.Payload, Email.Payload, Pub.Payload);
    }
}