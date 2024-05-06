using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

public class UnMuteUserCommand : Command<UserID>, ICommand<UnMuteUserCommand> {
    private UnMuteUserCommand(UserID userId, UserID mutedUserId) : base(userId) {
        MutedUserId = mutedUserId;
    }

    public UserID MutedUserId { get; }

    public static int ParametersCount { get; } = 2;

    public static Result<UnMuteUserCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
            return Error.WrongNumberOfParameters;

        var errors = new HashSet<Error>();

        var userId = UserID.Create(args[0].ToString());
        if (userId.IsFailure) errors.Add(userId.Error);

        var mutedUserId = UserID.Create(args[1].ToString());
        if (mutedUserId.IsFailure) errors.Add(mutedUserId.Error);

        if (errors.Any()) return Error.CompileErrors(errors);

        return new UnMuteUserCommand(userId.Payload, mutedUserId.Payload);
    }
}