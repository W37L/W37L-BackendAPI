using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.User;

/// <summary>
/// Handles the command for following a user.
/// </summary>
public class FollowAUserHanldler(
    IInteractionRepository interactionRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<FollowAUserCommand> {
    
    /// <summary>
    /// Handles the command to follow a user asynchronously.
    /// </summary>
    /// <param name="command">The command to follow a user.</param>
    /// <returns>A result indicating the success or failure of the operation.</returns>
    public async Task<Result> HandleAsync(FollowAUserCommand command) {
        // Search for user by id
        var result = await userRepository.GetByIdAsync(command.UserId);
        if (result.IsFailure)
            return Error.UserNotFound;

        // Search for user to follow by id
        var userToFollow = await userRepository.GetByIdAsync(command.UserToFollowId);
        if (userToFollow.IsFailure)
            return Error.UserNotFound;

        // Follow user Local
        var followResult = result.Payload.Follow(userToFollow.Payload);

        if (followResult.IsFailure)
            return followResult;

        // Update user in the database
        var res = interactionRepository.FollowUserAsync(result.Payload.Id.Value, userToFollow.Payload.Id.Value).Result
            .OnSuccess(() => {
                userRepository.IncrementFollowingAsync(result.Payload.Id.Value);
                userRepository.IncrementFollowersAsync(userToFollow.Payload.Id.Value);
                unitOfWork.SaveChangesAsync();
            });

        if (res.IsFailure)
            return res;

        return Result.Success();
    }
}