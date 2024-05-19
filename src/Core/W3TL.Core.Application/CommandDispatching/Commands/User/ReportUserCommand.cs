using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

/// <summary>
///  Represents a command to report a user.
/// </summary>
public class ReportUserCommand : Command<UserID>, ICommand<ReportUserCommand> {
    
    /// <summary>
    ///  Initializes a new instance of the <see cref="ReportUserCommand"/> class.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="reportedUserId"></param>
    private ReportUserCommand(UserID userId, UserID reportedUserId) : base(userId) {
        ReportedUserId = reportedUserId;
    }

    /// <summary>
    ///  The ID of the user to be reported.
    /// </summary>
    public UserID ReportedUserId { get; }

    /// <summary>
    ///     The number of parameters required to create a <see cref="ReportUserCommand"/>.
    /// </summary>
    public static int ParametersCount { get; } = 2;

    /// <summary>
    ///   Creates a new <see cref="ReportUserCommand"/> instance if the provided arguments are valid.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static Result<ReportUserCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
            return Error.WrongNumberOfParameters;

        var errors = new HashSet<Error>();

        var userId = UserID.Create(args[0].ToString());
        if (userId.IsFailure) errors.Add(userId.Error);

        var reportedUserId = UserID.Create(args[1].ToString());
        if (reportedUserId.IsFailure) errors.Add(reportedUserId.Error);

        if (errors.Any()) return Error.CompileErrors(errors);

        return new ReportUserCommand(userId.Payload, reportedUserId.Payload);
    }
}