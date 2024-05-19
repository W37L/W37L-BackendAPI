using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

/// <summary>
///  Represents a command to block a user.
/// </summary>
public class BlockUserCommand : Command<UserID>, ICommand<BlockUserCommand> {
    
    /// <summary>
    ///  Initializes a new instance of the <see cref="BlockUserCommand"/> class.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="blockedUserId"></param>
    private BlockUserCommand(UserID userId, UserID blockedUserId) : base(userId) {
        BlockedUserId = blockedUserId;
    }

    /// <summary>
    ///  The ID of the user who is blocking another user.
    /// </summary>
    public UserID BlockedUserId { get; }

    /// <summary>
    ///     The number of parameters required to create a <see cref="BlockUserCommand"/>.
    /// </summary>
    public static int ParametersCount { get; } = 2;

    /// <summary>
    ///  Creates a new <see cref="BlockUserCommand"/> instance if the provided arguments are valid.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static Result<BlockUserCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
            return Error.WrongNumberOfParameters;

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