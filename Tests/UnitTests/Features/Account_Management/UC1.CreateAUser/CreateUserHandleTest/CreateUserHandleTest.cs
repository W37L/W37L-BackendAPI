using UnitTests.Common.Factories;
using UnitTests.Fakes;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.Features.User;
using W3TL.Core.Domain.Common.Values;

namespace UnitTests.Features.Account_Management.UC1.CreateAUser.CreateUserHandleTest;

public class CreateUserHandleTest {
    // Test to check if the user is created successfully
    //ID:UC1.S1
    [Fact]
    public void Create_ValidInput_Success() {
        // Arrange
        var userId = ValidFields.VALID_USER_ID;
        var userName = ValidFields.VALID_USERNAME;
        var firstName = ValidFields.VALID_FIRST_NAME;
        var lastName = ValidFields.VALID_LAST_NAME;
        var email = ValidFields.VALID_EMAIL;
        var pubKey = ValidFields.VALID_PUB_KEY;

        var command = CreateUserCommand.Create(userId!, userName!, firstName!, lastName!, email!, pubKey!).Payload;
        var uow = new FakeUoW();
        var userRepository = new InMemUserRepoStub();

        var handler = new CreateUserHandler(userRepository, uow);

        // Act
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(userId, userRepository.GetByIdAsync(UserID.Create(userId!).Payload).Result.Payload.Id.Value);
        Assert.Equal(userName, userRepository.GetByIdAsync(UserID.Create(userId!).Payload).Result.Payload.UserName.Value);
        Assert.Equal(firstName, userRepository.GetByIdAsync(UserID.Create(userId!).Payload).Result.Payload.FirstName.Value);
        Assert.Equal(lastName, userRepository.GetByIdAsync(UserID.Create(userId!).Payload).Result.Payload.LastName.Value);
        Assert.Equal(email, userRepository.GetByIdAsync(UserID.Create(userId!).Payload).Result.Payload.Email.Value);
        Assert.Equal(pubKey, userRepository.GetByIdAsync(UserID.Create(userId!).Payload).Result.Payload.Pub.Value);
    }

    // Test to check if the user is not created successfully
    //ID:UC1.F1
    [Fact]
    public async void Create_UserAlreadyRegistered_Failure() {
        // Arrange
        var userId = ValidFields.VALID_USER_ID;
        var userName = ValidFields.VALID_USERNAME;
        var firstName = ValidFields.VALID_FIRST_NAME;
        var lastName = ValidFields.VALID_LAST_NAME;
        var email = ValidFields.VALID_EMAIL;
        var pubKey = ValidFields.VALID_PUB_KEY;

        var command = CreateUserCommand.Create(userId!, userName!, firstName!, lastName!, email!, pubKey!).Payload;
        var uow = new FakeUoW();
        var userRepository = new InMemUserRepoStub();

        var handler = new CreateUserHandler(userRepository, uow);

        // Act
        await handler.HandleAsync(command);
        var result = handler.HandleAsync(command).Result;

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.UserAlreadyRegistered, result.Error.EnumerateAll());
    }
}