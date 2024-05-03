using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.User;

public class CreateUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateUserCommand> {
    public async Task<Result> HandleAsync(CreateUserCommand command) {
        // Search for user by id
        var result = await userRepository.GetByIdAsync(command.Id);
        if (result.IsSuccess)
            return Error.UserAlreadyRegistered;

        // Search for user by email, and username
        var userResult = await userRepository.GetByEmailAsync(command.Email.Value);
        if (userResult.IsSuccess)
            return Error.EmailAlreadyRegistered;

        userResult = await userRepository.GetByUserNameAsync(command.UserName.Value);
        if (userResult.IsSuccess)
            return Error.UserNameAlreadyRegistered;

        // Create user
        var user = global::User.Create(command.UserId, command.UserName, command.FirstName, command.LastName,
            command.Email, command.Pub);

        if (user.IsFailure)
            return user.Error;

        // Add user to repository
        await userRepository.AddAsync(user.Payload);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}