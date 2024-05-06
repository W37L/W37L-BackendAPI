using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.User;

public class UnReportUserHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IInteractionRepository interactionRepository)
    : ICommandHandler<UnReportUserCommand> {
    public async Task<Result> HandleAsync(UnReportUserCommand command) {
        // Search for user by id
        var result = await userRepository.GetByIdAsync(command.Id);
        if (result.IsFailure)
            return Error.UserNotFound;

        // Search for reported user by id
        var reportedUser = await userRepository.GetByIdAsync(command.ReportedUserId);
        if (reportedUser.IsFailure)
            return Error.UserNotFound;

        // Unreport user
        var unreport = result.Payload.Unreport(reportedUser.Payload);

        if (unreport.IsFailure)
            return unreport.Error;

        // Add unreport to repository
        await interactionRepository.UnreportUserAsync(result.Payload.Id.Value, reportedUser.Payload.Id.Value);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}