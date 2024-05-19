using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.User.Values;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

/// <summary>
///  Represents a command to create a user.
/// </summary>
public class CreateUserCommand : Command<UserID>, ICommand<CreateUserCommand> {
    
    /// <summary>
    ///  Initializes a new instance of the <see cref="CreateUserCommand"/> class.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="userName"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="email"></param>
    /// <param name="pub"></param>
    private CreateUserCommand(UserID userId, UserNameType userName, NameType firstName, LastNameType lastName, EmailType email, PubType pub) : base(userId) {
        UserId = userId;
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Pub = pub;
    }

    /// <summary>
    ///  The ID of the user to be created.
    /// </summary>
    public UserID UserId { get; }
    
    /// <summary>
    ///  The username of the user to be created.
    /// </summary>
    public UserNameType UserName { get; }
    
    /// <summary>
    ///  The first name of the user to be created.
    /// </summary>
    public NameType FirstName { get; }
    
    /// <summary>
    ///  The last name of the user to be created.
    /// </summary>
    public LastNameType LastName { get; }
    
    /// <summary>
    ///  The email of the user to be created.
    /// </summary>
    public EmailType Email { get; }
    
    /// <summary>
    ///  The pub of the user to be created.
    /// </summary>
    public PubType Pub { get; }

    /// <summary>
    ///  The number of parameters required to create a <see cref="CreateUserCommand"/>.
    /// </summary>
    public static int ParametersCount { get; } = 6;

    /// <summary>
    ///  Creates a new <see cref="CreateUserCommand"/> instance if the provided arguments are valid.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
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