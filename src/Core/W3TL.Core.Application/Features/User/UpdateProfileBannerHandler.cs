using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.User;

/// <summary>
/// Handles the command for updating a user's profile banner.
/// </summary>
public class UpdateProfileBannerHandler(
    IUserRepository userRepository, 
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateProfileBannerCommand> {
    
    /// <summary>
    /// Handles the command to update a user's profile banner asynchronously.
    /// </summary>
    /// <param name="command">The command to update a user's profile banner.</param>
    /// <returns>A result indicating the success or failure of the operation.</returns>
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