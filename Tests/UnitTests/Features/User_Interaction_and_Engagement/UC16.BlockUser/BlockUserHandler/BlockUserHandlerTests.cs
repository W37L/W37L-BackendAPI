using UnitTests.Common.Factories;
using UnitTests.Fakes;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.Features.User;

public class BlockUserHandlerTests {
    private readonly BlockUserHandler _handler;
    private readonly InMemInteractionRepoStub _interactionRepository = new();
    private readonly FakeUoW _unitOfWork = new();
    private readonly InMemUserRepoStub _userRepository = new();

    public BlockUserHandlerTests() {
        _handler = new BlockUserHandler(_userRepository, _unitOfWork, _interactionRepository);
    }

    [Fact]
    public async Task HandleAsync_WhenUsersExist_BlocksSuccessfully() {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();
        var userToBlock = UserFactory.InitWithDefaultValues().Build();
        await _userRepository.AddAsync(user);
        await _userRepository.AddAsync(userToBlock);

        var command = BlockUserCommand.Create(user.Id.Value, userToBlock.Id.Value).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task HandleAsync_UserNotFound_ReturnsFailure() {
        // Arrange
        var nonExistentUserId = ValidFields.VALID_USER_ID;
        var nonExistentBlockedUserId = UserFactory.InitWithDefaultValues().Build().Id.Value;
        var command = BlockUserCommand.Create(nonExistentUserId, nonExistentBlockedUserId).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.UserNotFound, result.Error.EnumerateAll());
    }
}