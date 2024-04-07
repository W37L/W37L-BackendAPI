using UnitTests.Common.Factories;
using UnitTests.Fakes;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.Features.User;

public class UnblockUserHandlerTests {
    private readonly UnblockUserHandler _handler;
    private readonly FakeUoW _unitOfWork = new();
    private readonly InMemUserRepoStub _userRepository = new();

    public UnblockUserHandlerTests() {
        _handler = new UnblockUserHandler(_userRepository, _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_WhenUsersExist_UnblocksSuccessfully() {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();
        var userToUnblock = UserFactory.InitWithDefaultValues().Build();
        await _userRepository.AddAsync(user);
        await _userRepository.AddAsync(userToUnblock);
        user.Block(userToUnblock); // Ensure the user is initially blocked

        var command = UnblockUserCommand.Create(user.Id.Value, userToUnblock.Id.Value).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        // Optionally, verify that the user's block list no longer includes the unblocked user
    }

    [Fact]
    public async Task HandleAsync_UserNotFound_ReturnsFailure() {
        // Arrange
        var nonExistentUserId = UserFactory.InitWithDefaultValues().Build().Id.Value;
        var nonExistentUnblockedUserId = ValidFields.VALID_USER_ID;
        var command = UnblockUserCommand.Create(nonExistentUserId, nonExistentUnblockedUserId).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        // Error should indicate the user or the user to be unblocked was not found
    }
}