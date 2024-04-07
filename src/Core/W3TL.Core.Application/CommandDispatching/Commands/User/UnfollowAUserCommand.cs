using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

public class UnfollowAUserCommand : Command<UserID>, ICommand<UnfollowAUserCommand> {
    private UnfollowAUserCommand(UserID userId, UserID userToUnFollowId) : base(userId) {
        UserId = userId;
        UserToUnFollowId = userToUnFollowId;
    }

    public UserID UserId { get; }
    public UserID UserToUnFollowId { get; }

    public static int ParametersCount { get; } = 2;

    public static Result<UnfollowAUserCommand> Create(params object[] args) {
        if (args.Length != ParametersCount) return Error.InvalidCommand;

        var errors = new HashSet<Error>();

        var userId = UserID.Create(args[0].ToString());
        if (userId.IsFailure) errors.Add(userId.Error);

        var userToUnFollowId = UserID.Create(args[1].ToString());
        if (userToUnFollowId.IsFailure) errors.Add(userToUnFollowId.Error);

        if (errors.Any()) return Error.CompileErrors(errors);

        return new UnfollowAUserCommand(userId.Payload, userToUnFollowId.Payload);
    }
}