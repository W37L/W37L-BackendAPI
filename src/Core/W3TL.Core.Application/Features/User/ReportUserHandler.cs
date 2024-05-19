using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.User;

/// <summary>
/// Handles the command for reporting a user.
/// </summary>
public class ReportUserHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IInteractionRepository interactionRepository) : ICommandHandler<ReportUserCommand> {
    
    /// <summary>
    /// Handles the command to report a user asynchronously.
    /// </summary>
    /// <param name="command">The command to report a user.</param>
    /// <returns>A result indicating the success or failure of the operation.</returns>
    public async Task<Result> HandleAsync(ReportUserCommand command) {
        // Search for user by id
        var result = await userRepository.GetByIdAsync(command.Id);
        if (result.IsFailure)
            return Error.UserNotFound;

        // Search for reported user by id
        var reportedUser = await userRepository.GetByIdAsync(command.ReportedUserId);
        if (reportedUser.IsFailure)
            return Error.UserNotFound;

        // Report user
        var report = result.Payload.Report(reportedUser.Payload);

        if (report.IsFailure)
            return report.Error;

        // Add report to repository
        await interactionRepository.ReportUserAsync(result.Payload.Id.Value, reportedUser.Payload.Id.Value);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}