using W3TL.Core.Domain.Agregates.User.Entity.Values;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

public class UpdateAvatarUserCommand : Command<UserID>, ICommand<UpdateAvatarUserCommand> {
    private UpdateAvatarUserCommand(UserID id, AvatarType avatar) : base(id) {
        Avatar = avatar;
    }

    public AvatarType Avatar { get; }

    public static int ParametersCount { get; } = 2;

    public static Result<UpdateAvatarUserCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
            return Error.InvalidCommand;

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