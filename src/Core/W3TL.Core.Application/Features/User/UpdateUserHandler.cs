using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.User;

public class UpdateUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserCommand> {
    public async Task<Result> HandleAsync(UpdateUserCommand command) {
        // Search for user by id
        var result = await userRepository.GetByIdAsync(command.Id);
        if (result.IsFailure)
            return Error.UserNotFound;
        var user = result.Payload;

        // Update user
        HashSet<Error> errors = new();

        user.UpdateUserName(command.UserName)
            .OnFailure(error => errors.Add(error));
        user.UpdateFirstName(command.FirstName)
            .OnFailure(error => errors.Add(error));
        user.UpdateLastName(command.LastName)
            .OnFailure(error => errors.Add(error));
        user.Profile?.UpdateBio(command.Bio)
            .OnFailure(error => errors.Add(error));
        user.Profile?.UpdateLocation(command.Location)
            .OnFailure(error => errors.Add(error));
        user.Profile?.UpdateWebsite(command.Website)
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.CompileErrors(errors);

        // Update user in repository
        await userRepository.UpdateAsync(user);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}