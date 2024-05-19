using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.User;

/// <summary>
/// Handles the command for unmuting a user.
/// </summary>
public class UnMuteUserHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IInteractionRepository interactionRepository) : ICommandHandler<UnMuteUserCommand> {
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UnMuteUserHandler"/> class.
    /// </summary>
    /// <param name="userRepository">The repository for accessing user data.</param>
    /// <param name="unitOfWork">The unit of work for managing transactions.</param>
    /// <param name="interactionRepository">The repository for interactions (e.g., muting).</param>
    public async Task<Result> HandleAsync(UnMuteUserCommand command) {
        // Search for user by id
        var result = await userRepository.GetByIdAsync(command.Id);
        if (result.IsFailure)
            return Error.UserNotFound;

        // Search for muted user by id
        var mutedUser = await userRepository.GetByIdAsync(command.MutedUserId);
        if (mutedUser.IsFailure)
            return Error.UserNotFound;

        // Unmute user
        var unMute = result.Payload.Unmute(mutedUser.Payload);

        if (unMute.IsFailure)
            return unMute.Error;

        // Add unmute to repository
        await interactionRepository.UnmuteUserAsync(result.Payload.Id.Value, mutedUser.Payload.Id.Value);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}