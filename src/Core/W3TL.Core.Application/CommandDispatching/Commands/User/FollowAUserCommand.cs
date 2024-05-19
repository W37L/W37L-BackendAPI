using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

/// <summary>
///   Represents a command to follow a user.
/// </summary>
public class FollowAUserCommand : Command<UserID>, ICommand<FollowAUserCommand> {
    
    /// <summary>
    ///  Initializes a new instance of the <see cref="FollowAUserCommand"/> class.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="userToFollowId"></param>
    private FollowAUserCommand(UserID userId, UserID userToFollowId) : base(userId) {
        UserId = userId;
        UserToFollowId = userToFollowId;
    }

    /// <summary>
    ///  The ID of the user who is following another user.
    /// </summary>
    public UserID UserId { get; }
    
    /// <summary>
    ///  The ID of the user to be followed.
    /// </summary>
    public UserID UserToFollowId { get; }

    /// <summary>
    ///  The number of parameters required to create a <see cref="FollowAUserCommand"/>.
    /// </summary>
    public static int ParametersCount { get; } = 2;

    /// <summary>
    ///  Creates a new <see cref="FollowAUserCommand"/> instance if the provided arguments are valid.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static Result<FollowAUserCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
            return Error.WrongNumberOfParameters;

        var errors = new HashSet<Error>();

        var userId = UserID.Create(args[0].ToString());
        if (userId.IsFailure) errors.Add(userId.Error);

        var userToFollowId = UserID.Create(args[1].ToString());
        if (userToFollowId.IsFailure) errors.Add(userToFollowId.Error);

        if (errors.Any()) return Error.CompileErrors(errors);

        return new FollowAUserCommand(userId.Payload, userToFollowId.Payload);
    }
}