using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.User;

/// <summary>
/// Handles the command for blocking a user.
/// </summary>
public class BlockUserHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IInteractionRepository interactionRepository) : ICommandHandler<BlockUserCommand> {
    
    /// <summary>
    /// Handles the command to block a user asynchronously.
    /// </summary>
    /// <param name="command">The command to block a user.</param>
    /// <returns>A result indicating the success or failure of the operation.</returns>
    public async Task<Result> HandleAsync(BlockUserCommand command) {
        // Search for user by id
        var result = await userRepository.GetByIdAsync(command.Id);
        if (result.IsFailure)
            return Error.UserNotFound;

        // Search for blocked user by id
        var blockedUser = await userRepository.GetByIdAsync(command.BlockedUserId);
        if (blockedUser.IsFailure)
            return Error.UserNotFound;

        // Block user
        var block = result.Payload.Block(blockedUser.Payload);

        if (block.IsFailure)
            return block.Error;

        // Add block to repository
        await interactionRepository.BlockUserAsync(result.Payload.Id.Value, blockedUser.Payload.Id.Value);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}