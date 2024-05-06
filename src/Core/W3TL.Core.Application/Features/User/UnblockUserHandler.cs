using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.User;

public class UnblockUserHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IInteractionRepository interactionRepository)
    : ICommandHandler<UnblockUserCommand> {
    public async Task<Result> HandleAsync(UnblockUserCommand command) {
        // Search for user by id
        var result = await userRepository.GetByIdAsync(command.Id);
        if (result.IsFailure)
            return Error.UserNotFound;

        // Search for blocked user by id
        var blockedUser = await userRepository.GetByIdAsync(command.UnblockedUserId);
        if (blockedUser.IsFailure)
            return Error.UserNotFound;

        // Unblock user
        var unblock = result.Payload.Unblock(blockedUser.Payload);

        if (unblock.IsFailure)
            return unblock.Error;

        // Add unblock to repository
        await interactionRepository.UnblockUserAsync(result.Payload.Id.Value, blockedUser.Payload.Id.Value);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}