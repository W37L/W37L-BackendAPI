using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

/// <summary>
///  Represents a command to unblock a user.
/// </summary>
public class UnblockUserCommand : Command<UserID>, ICommand<UnblockUserCommand> {
    
    /// <summary>
    ///  Initializes a new instance of the <see cref="UnblockUserCommand"/> class.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="unblockedUserId"></param>
    private UnblockUserCommand(UserID userId, UserID unblockedUserId) : base(userId) {
        UnblockedUserId = unblockedUserId;
    }

    /// <summary>
    ///     The ID of the user to be unblocked.
    /// </summary>
    public UserID UnblockedUserId { get; }

    /// <summary>
    ///   The number of parameters required to create a <see cref="UnblockUserCommand"/>.
    /// </summary>
    public static int ParametersCount { get; } = 2;

    /// <summary>
    ///  Creates a new <see cref="UnblockUserCommand"/> instance if the provided arguments are valid.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static Result<UnblockUserCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
            return Error.WrongNumberOfParameters;

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