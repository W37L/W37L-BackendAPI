using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

/// <summary>
///  Represents a command to unfollow a user.
/// </summary>
public class UnfollowAUserCommand : Command<UserID>, ICommand<UnfollowAUserCommand> {
    
    /// <summary>
    ///  Initializes a new instance of the <see cref="UnfollowAUserCommand"/> class.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="userToUnFollowId"></param>
    private UnfollowAUserCommand(UserID userId, UserID userToUnFollowId) : base(userId) {
        UserId = userId;
        UserToUnFollowId = userToUnFollowId;
    }

    /// <summary>
    ///     
    /// </summary>
    public UserID UserId { get; }
    
    /// <summary>
    ///     
    /// </summary>
    public UserID UserToUnFollowId { get; }

    /// <summary>
    ///  The number of parameters required to create a <see cref="UnfollowAUserCommand"/>.
    /// </summary>
    public static int ParametersCount { get; } = 2;

    /// <summary>
    ///  Creates a new <see cref="UnfollowAUserCommand"/> instance if the provided arguments are valid.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static Result<UnfollowAUserCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
            return
                Error.WrongNumberOfParameters;

        var errors = new HashSet<Error>();

        var userId = UserID.Create(args[0].ToString());
        if (userId.IsFailure) errors.Add(userId.Error);

        var userToUnFollowId = UserID.Create(args[1].ToString());
        if (userToUnFollowId.IsFailure) errors.Add(userToUnFollowId.Error);

        if (errors.Any()) return Error.CompileErrors(errors);

        return new UnfollowAUserCommand(userId.Payload, userToUnFollowId.Payload);
    }
}