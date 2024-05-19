using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.User;

/// <summary>
/// Handles the command for muting a user.
/// </summary>
public class MuteUserHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IInteractionRepository interactionRepository) : ICommandHandler<MuteUserCommand> {
    
    /// <summary>
    /// Handles the command to mute a user asynchronously.
    /// </summary>
    /// <param name="command">The command to mute a user.</param>
    /// <returns>A result indicating the success or failure of the operation.</returns>
    public async Task<Result> HandleAsync(MuteUserCommand command) {
        // Search for user by id
        var result = await userRepository.GetByIdAsync(command.Id);
        if (result.IsFailure)
            return Error.UserNotFound;

        // Search for muted user by id
        var mutedUser = await userRepository.GetByIdAsync(command.MutedUserId);
        if (mutedUser.IsFailure)
            return Error.UserNotFound;

        // Mute user
        var mute = result.Payload.Mute(mutedUser.Payload);

        if (mute.IsFailure)
            return mute.Error;

        // Add mute to repository
        await interactionRepository.MuteUserAsync(result.Payload.Id.Value, mutedUser.Payload.Id.Value);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}