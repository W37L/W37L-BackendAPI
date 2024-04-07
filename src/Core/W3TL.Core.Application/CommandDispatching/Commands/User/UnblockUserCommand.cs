using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

public class UnblockUserCommand : Command<UserID>, ICommand<UnblockUserCommand> {
    private UnblockUserCommand(UserID userId, UserID unblockedUserId) : base(userId) {
        UnblockedUserId = unblockedUserId;
    }

    public UserID UnblockedUserId { get; }

    public static int ParametersCount { get; } = 2;

    public static Result<UnblockUserCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
            return Error.InvalidCommand;

        var errors = new HashSet<Error>();

        var userIdResult = UserID.Create(args[0].ToString())
            .OnFailure(error => errors.Add(error));

        var unblockedUserIdResult = UserID.Create(args[1].ToString())
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.CompileErrors(errors);

        return new UnblockUserCommand(userIdResult.Payload, unblockedUserIdResult.Payload);
    }
}