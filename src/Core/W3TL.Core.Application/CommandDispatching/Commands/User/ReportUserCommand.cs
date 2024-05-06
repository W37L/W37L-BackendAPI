using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

public class ReportUserCommand : Command<UserID>, ICommand<ReportUserCommand> {
    private ReportUserCommand(UserID userId, UserID reportedUserId) : base(userId) {
        ReportedUserId = reportedUserId;
    }

    public UserID ReportedUserId { get; }

    public static int ParametersCount { get; } = 2;

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