using UnitTests.Common.Factories;
using UnitTests.Fakes;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.Features.User;

public class FollowAUserHandlerTests {
    private readonly FollowAUserHanldler _handler;
    private readonly FakeUoW _unitOfWork = new();
    private readonly InMemUserRepoStub _userRepository = new();

    public FollowAUserHandlerTests() {
        _handler = new FollowAUserHanldler(_userRepository, _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_WhenUsersExist_FollowsSuccessfully() {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();
        var userToFollow = UserFactory
            .Init()
            .WithValidUserName("userToFollow")
            .WithValidEmail("valid@email.com")
            .WithValidFirstName("John")
            .WithValidLastName("Doe")
            .Build();

        await _userRepository.AddAsync(user);
        await _userRepository.AddAsync(userToFollow);

        var command = FollowAUserCommand.Create(user.Id.Value, userToFollow.Id.Value).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(userToFollow, user.Following);
        Assert.Contains(user, userToFollow.Followers);
    }

    [Fact]
    public async Task HandleAsync_UserNotFound_ReturnsFailure() {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();
        await _userRepository.AddAsync(user);

        var invalidUserToFollowId = UserFactory
            .Init()
            .WithValidUserName("userToFollow")
            .WithValidEmail("valid@email.com")
            .WithValidFirstName("John")
            .WithValidLastName("Doe")
            .Build();

        var command = FollowAUserCommand.Create(user.Id.Value, invalidUserToFollowId.Id.Value).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.UserNotFound, result.Error);
    }
}