using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

/// <summary>
///  Represents a command to unmute a user.
/// </summary>
public class UnMuteUserCommand : Command<UserID>, ICommand<UnMuteUserCommand> {
    
    /// <summary>
    ///  Initializes a new instance of the <see cref="UnMuteUserCommand"/> class.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="mutedUserId"></param>
    private UnMuteUserCommand(UserID userId, UserID mutedUserId) : base(userId) {
        MutedUserId = mutedUserId;
    }

    /// <summary>
    ///     The ID of the user to be unmuted.
    /// </summary>
    public UserID MutedUserId { get; }

    /// <summary>
    ///   The number of parameters required to create a <see cref="UnMuteUserCommand"/>.
    /// </summary>
    public static int ParametersCount { get; } = 2;

    /// <summary>
    ///  Creates a new <see cref="UnMuteUserCommand"/> instance if the provided arguments are valid.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
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