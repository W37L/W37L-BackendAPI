using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.User;

public class UpdateProfileBannerHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateProfileBannerCommand> {
    public async Task<Result> HandleAsync(UpdateProfileBannerCommand command) {
        // Search for user by id
        var result = await userRepository.GetByIdAsync(command.Id);
        if (result.IsFailure)
            return Error.UserNotFound;
        var user = result.Payload;

        // Update user
        var updated = user.Profile.UpdateBanner(command.Banner);
        if (updated.IsFailure)
            return updated.Error;

        // Update user in repository
        await userRepository.UpdateAsync(user);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}