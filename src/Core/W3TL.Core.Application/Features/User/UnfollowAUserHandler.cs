using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

/// <summary>
/// Handles the command for unfollowing a user.
/// </summary>
public class UnfollowAUserHandler(
    IInteractionRepository interactionRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UnfollowAUserCommand> {
    
    /// <summary>
    /// Handles the command to unfollow a user asynchronously.
    /// </summary>
    /// <param name="command">The command to unfollow a user.</param>
    /// <returns>A result indicating the success or failure of the operation.</returns>
    public async Task<Result> HandleAsync(UnfollowAUserCommand command) {
        // Search for user by id
        var result = await userRepository.GetByIdAsync(command.UserId);
        if (result.IsFailure)
            return Error.UserNotFound;

        // Search for user to unfollow by id
        var userToUnfollow = await userRepository.GetByIdAsync(command.UserToUnFollowId);
        if (userToUnfollow.IsFailure)
            return Error.UserNotFound;

        // Unfollow user
        var unfollow = result.Payload.Unfollow(userToUnfollow.Payload);

        if (unfollow.IsFailure)
            return unfollow;

        // Update user
        var res = interactionRepository.UnfollowUserAsync(result.Payload.Id.Value, userToUnfollow.Payload.Id.Value)
            .Result
            .OnSuccess(() => {
                userRepository.DecrementFollowingAsync(result.Payload.Id.Value);
                userRepository.DecrementFollowersAsync(userToUnfollow.Payload.Id.Value);
                unitOfWork.SaveChangesAsync();
            });

        if (res.IsFailure)
            return res;

        return Result.Success();
    }
}