using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

public class BlockUserCommand : Command<UserID>, ICommand<BlockUserCommand> {
    private BlockUserCommand(UserID userId, UserID blockedUserId) : base(userId) {
        BlockedUserId = blockedUserId;
    }

    public UserID BlockedUserId { get; }

    public static int ParametersCount { get; } = 2;

    public static Result<BlockUserCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
            return Error.InvalidCommand;

        var errors = new HashSet<Error>();

        var userIdResult = UserID.Create(args[0].ToString())
            .OnFailure(error => errors.Add(error));

        var blockedUserIdResult = UserID.Create(args[1].ToString())
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.CompileErrors(errors);

        return new BlockUserCommand(userIdResult.Payload, blockedUserIdResult.Payload);
    }
}