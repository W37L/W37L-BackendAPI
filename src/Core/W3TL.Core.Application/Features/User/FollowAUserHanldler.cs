using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.User;

public class FollowAUserHanldler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<FollowAUserCommand> {
    public async Task<Result> HandleAsync(FollowAUserCommand command) {
        // Search for user by id
        var result = await userRepository.GetByIdAsync(command.UserId);
        if (result.IsFailure)
            return Error.UserNotFound;

        // Search for user to follow by id
        var userToFollow = await userRepository.GetByIdAsync(command.UserToFollowId);
        if (userToFollow.IsFailure)
            return Error.UserNotFound;

        // Follow user
        result.Payload.Follow(userToFollow.Payload);

        // Update user
        await userRepository.UpdateAsync(result.Payload);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}