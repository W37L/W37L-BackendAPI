using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

/// <summary>
///  Represents a command to unreport a user.
/// </summary>
public class UnReportUserCommand : Command<UserID>, ICommand<UnReportUserCommand> {
    
    /// <summary>
    ///  Initializes a new instance of the <see cref="UnReportUserCommand"/> class.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="reportedUserId"></param>
    private UnReportUserCommand(UserID userId, UserID reportedUserId) : base(userId) {
        ReportedUserId = reportedUserId;
    }

    /// <summary>
    ///     The ID of the user to be unreported.
    /// </summary>
    public UserID ReportedUserId { get; }

    /// <summary>
    ///  The number of parameters required to create a <see cref="UnReportUserCommand"/>.
    /// </summary>
    public static int ParametersCount { get; } = 2;

    /// <summary>
    ///  Creates a new <see cref="UnReportUserCommand"/> instance if the provided arguments are valid.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static Result<UnReportUserCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
            return Error.WrongNumberOfParameters;

        var errors = new HashSet<Error>();

        var userId = UserID.Create(args[0].ToString());
        if (userId.IsFailure) errors.Add(userId.Error);

        var reportedUserId = UserID.Create(args[1].ToString());
        if (reportedUserId.IsFailure) errors.Add(reportedUserId.Error);

        if (errors.Any()) return Error.CompileErrors(errors);

        return new UnReportUserCommand(userId.Payload, reportedUserId.Payload);
    }
}