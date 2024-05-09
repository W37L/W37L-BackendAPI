using UnitTests.Common.Factories;
using UnitTests.Fakes;
using W3TL.Core.Application.CommandDispatching.Commands.User;

public class UnfollowAUserHandlerTests {
    private readonly UnfollowAUserHandler _handler;
    private readonly InMemInteractionRepoStub _interactionRepository = new();
    private readonly FakeUoW _unitOfWork = new();
    private readonly InMemUserRepoStub _userRepository = new();

    public UnfollowAUserHandlerTests() {
        _handler = new UnfollowAUserHandler(_interactionRepository, _userRepository, _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_WhenUsersExist_UnfollowsSuccessfully() {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();
        var userToUnfollow = UserFactory.InitWithDefaultValues().Build();

        // Ensure user follows the userToUnfollow before the test
        user.Follow(userToUnfollow);

        await _userRepository.AddAsync(user);
        await _userRepository.AddAsync(userToUnfollow);

        var command = UnfollowAUserCommand.Create(user.Id.Value, userToUnfollow.Id.Value).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsFailure); // the user is in memort
        Assert.DoesNotContain(userToUnfollow.Id,
            user.Interactions.Following); // Assuming Following is the collection tracking who the user follows
    }

    [Fact]
    public async Task HandleAsync_UserNotFound_ReturnsFailure() {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();
        var nonExistentUserId = UserFactory.InitWithDefaultValues().Build();


        await _userRepository.AddAsync(user);

        var command = UnfollowAUserCommand.Create(user.Id.Value, nonExistentUserId.Id.Value).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.UserNotFollowed, result.Error.EnumerateAll());
    }
}