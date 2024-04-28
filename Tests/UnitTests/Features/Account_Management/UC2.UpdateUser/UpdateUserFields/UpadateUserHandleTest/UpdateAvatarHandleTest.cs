using UnitTests.Common.Factories;
using UnitTests.Fakes;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.Features.User;
using W3TL.Core.Domain.Common.Values;

public class UpdateAvatarHandlerTests {
    private readonly UpdateAvatarHandler _handler;
    private readonly FakeUoW _unitOfWork;
    private readonly InMemUserRepoStub _userRepository;

    public UpdateAvatarHandlerTests() {
        _userRepository = new InMemUserRepoStub();
        _unitOfWork = new FakeUoW();
        _handler = new UpdateAvatarHandler(_userRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_UserExists_Success() {
        // Arrange - Add a user to the in-memory repository for updating
        var user = UserFactory.InitWithDefaultValues().Build();
        await _userRepository.AddAsync(user);

        var avatarUrl = "http://example.com/new-avatar.jpg";
        var command = UpdateAvatarUserCommand.Create(user.Id.Value, avatarUrl).Payload;


        // Act - Update the user's avatar
        var result = await _handler.HandleAsync(command);

        // Assert - Verify the update was successful
        Assert.True(result.IsSuccess);
        var updatedUserResult = await _userRepository.GetByIdAsync(user.Id);
        Assert.Equal(avatarUrl, updatedUserResult.Payload.Profile.Avatar.Url);
    }

    [Fact]
    public async Task Handle_UserDoesNotExist_Failure() {
        // Arrange - Ensure the user does not exist in the in-memory repository
        var userId = UserID.Generate().Payload.Value; // A new user ID
        var avatarUrl = "http://example.com/new-avatar.jpg";
        var command = UpdateAvatarUserCommand.Create(userId, avatarUrl).Payload;

        // Act - Attempt to update a non-existing user's avatar
        var result = await _handler.HandleAsync(command);

        // Assert - Verify that the update fails due to user not found
        Assert.True(result.IsFailure);
        Assert.Equal(Error.UserNotFound, result.Error);
    }
}