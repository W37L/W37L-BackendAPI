using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.Features.User;

public class CreateUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<Command<UserID>> {
    public async Task<Result> HandleAsync(Command<UserID> command) {
        var result = await userRepository.GetByIdAsync(command.Id);

        if (result.IsSuccess)
            return Error.UserAlreadyRegistered;

        if (command is CreateUserCommand createUserCommand) {
            var user = global::User.Create(createUserCommand.Id, createUserCommand.UserName, createUserCommand.FirstName, createUserCommand.LastName, createUserCommand.Email, createUserCommand.Pub);

            if (user.IsFailure)
                return user.Error;

            await userRepository.AddAsync(user.Payload);
            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        return Error.InvalidCommand;
    }
}