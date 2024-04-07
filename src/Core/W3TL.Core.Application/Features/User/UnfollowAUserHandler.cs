using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

public class UnfollowAUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<UnfollowAUserCommand> {
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
        result.Payload.Unfollow(userToUnfollow.Payload);

        // Update user
        await userRepository.UpdateAsync(result.Payload);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}