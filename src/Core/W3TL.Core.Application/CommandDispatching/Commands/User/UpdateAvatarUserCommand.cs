using W3TL.Core.Domain.Agregates.User.Entity.Values;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

/// <summary>
///  Represents a command to update the avatar of a user.
/// </summary>
public class UpdateAvatarUserCommand : Command<UserID>, ICommand<UpdateAvatarUserCommand> {
    
    /// <summary>
    ///  Initializes a new instance of the <see cref="UpdateAvatarUserCommand"/> class.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="avatar"></param>
    private UpdateAvatarUserCommand(UserID id, AvatarType avatar) : base(id) {
        Avatar = avatar;
    }

    /// <summary>
    ///  The avatar of the user.
    /// </summary>
    public AvatarType Avatar { get; }

    /// <summary>
    ///  The number of parameters required to create a <see cref="UpdateAvatarUserCommand"/>.
    /// </summary>
    public static int ParametersCount { get; } = 2;

    /// <summary>
    ///  Creates a new <see cref="UpdateAvatarUserCommand"/> instance if the provided arguments are valid.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static Result<UpdateAvatarUserCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
            return Error.WrongNumberOfParameters;

        var errors = new HashSet<Error>();

        var userId = UserID.Create(args[0].ToString())
            .OnFailure(error => errors.Add(error));

        var avatar = AvatarType.Create(args[1].ToString())
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.CompileErrors(errors);

        return new UpdateAvatarUserCommand(userId.Payload, avatar.Payload);
    }
}