using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

public class FollowAUserCommand : Command<UserID>, ICommand<FollowAUserCommand> {
    private FollowAUserCommand(UserID userId, UserID userToFollowId) : base(userId) {
        UserId = userId;
        UserToFollowId = userToFollowId;
    }

    public UserID UserId { get; }
    public UserID UserToFollowId { get; }

    public static int ParametersCount { get; } = 2;

    public static Result<FollowAUserCommand> Create(params object[] args) {
        if (args.Length != ParametersCount) return Error.InvalidCommand;

        var errors = new HashSet<Error>();

        var userId = UserID.Create(args[0].ToString());
        if (userId.IsFailure) errors.Add(userId.Error);

        var userToFollowId = UserID.Create(args[1].ToString());
        if (userToFollowId.IsFailure) errors.Add(userToFollowId.Error);

        if (errors.Any()) return Error.CompileErrors(errors);

        return new FollowAUserCommand(userId.Payload, userToFollowId.Payload);
    }
}