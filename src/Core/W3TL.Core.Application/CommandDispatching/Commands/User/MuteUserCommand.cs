using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

/// <summary>
///  Represents a command to mute a user.
/// </summary>
public class MuteUserCommand : Command<UserID>, ICommand<MuteUserCommand> {
    
    /// <summary>
    ///  Initializes a new instance of the <see cref="MuteUserCommand"/> class.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="mutedUserId"></param>
    private MuteUserCommand(UserID userId, UserID mutedUserId) : base(userId) {
        MutedUserId = mutedUserId;
    }

    /// <summary>
    ///  The ID of the user to be muted.
    /// </summary>
    public UserID MutedUserId { get; }

    /// <summary>
    ///  The number of parameters required to create a <see cref="MuteUserCommand"/>.
    /// </summary>
    public static int ParametersCount { get; } = 2;

    /// <summary>
    ///  Creates a new <see cref="MuteUserCommand"/> instance if the provided arguments are valid.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static Result<MuteUserCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
            return Error.WrongNumberOfParameters;

        var errors = new HashSet<Error>();

        var userId = UserID.Create(args[0].ToString());
        if (userId.IsFailure) errors.Add(userId.Error);

        var mutedUserId = UserID.Create(args[1].ToString());
        if (mutedUserId.IsFailure) errors.Add(mutedUserId.Error);

        if (errors.Any()) return Error.CompileErrors(errors);

        return new MuteUserCommand(userId.Payload, mutedUserId.Payload);
    }
}