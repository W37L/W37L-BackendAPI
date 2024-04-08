using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.User.Values;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

public class CreateUserCommand : Command<UserID>, ICommand<CreateUserCommand> {
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

    public static int ParametersCount { get; } = 6;

    public static Result<CreateUserCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
            return Error.WrongNumberOfParameters;

        var errors = new HashSet<Error>();

        var userId = UserID.Create(args[0].ToString())
            .OnFailure(error => errors.Add(error));

        var userName = UserNameType.Create(args[1].ToString())
            .OnFailure(error => errors.Add(error));

        var firstName = NameType.Create(args[2].ToString())
            .OnFailure(error => errors.Add(error));

        var lastName = LastNameType.Create(args[3].ToString())
            .OnFailure(error => errors.Add(error));

        var email = EmailType.Create(args[4].ToString())
            .OnFailure(error => errors.Add(error));

        var pub = PubType.Create(args[5].ToString())
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.CompileErrors(errors);


        return new CreateUserCommand(userId.Payload, userName.Payload, firstName.Payload, lastName.Payload, email.Payload, pub.Payload);
    }
}