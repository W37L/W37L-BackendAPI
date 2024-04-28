using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using W3TL.Core.Domain.Agregates.User.Entity.Values;
using W3TL.Core.Domain.Agregates.User.Values;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

public class UpdateUserCommand : Command<UserID>, ICommand<UpdateUserCommand> {
    private UpdateUserCommand(UserID userId, UserNameType userName, NameType firstName, LastNameType lastName,
        BioType? bio, LocationType? location, WebsiteType? website) : base(userId) {
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        Bio = bio;
        Location = location;
        Website = website;
    }

    public UserNameType UserName { get; }
    public NameType FirstName { get; }
    public LastNameType LastName { get; }
    public BioType? Bio { get; }
    public LocationType? Location { get; }
    public WebsiteType? Website { get; }

    public static int ParametersCount { get; } = 7;

    public static Result<UpdateUserCommand> Create(params object[] args) {
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

        var bio = args[4] == null
            ? null
            : BioType.Create(args[4].ToString())
                .OnFailure(error => errors.Add(error)).Payload;


        var location = args[5] == null
            ? null
            : LocationType.Create(args[5].ToString())
                .OnFailure(error => errors.Add(error)).Payload;

        var website = args[6] == null
            ? null
            : WebsiteType.Create(args[6].ToString())
                .OnFailure(error => errors.Add(error)).Payload;


        if (errors.Any())
            return Error.CompileErrors(errors);

        return new UpdateUserCommand(userId.Payload, userName.Payload, firstName.Payload, lastName.Payload, bio,
            location, website);
    }
}