using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.User;

public class UpdateProfileBannerHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateProfileBannerCommand> {
    public async Task<Result> HandleAsync(UpdateProfileBannerCommand command) {
        // Search for user by id
        var result = await userRepository.ExistsAsync(command.Id);
        if (result.IsFailure)
            return result.Error;


        // Update user in repository
        await userRepository.UpdateFieldAsync(command.Id.Value, "banner", command.Banner.Url);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}