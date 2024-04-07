using UnitTests.Common.Factories;
using UnitTests.Fakes;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.Features.User;
using W3TL.Core.Domain.Common.Values;

public class UpdateProfileBannerHandlerTests {
    private readonly UpdateProfileBannerHandler _handler;
    private readonly FakeUoW _unitOfWork;
    private readonly InMemUserRepoStub _userRepository;

    public UpdateProfileBannerHandlerTests() {
        _userRepository = new InMemUserRepoStub();
        _unitOfWork = new FakeUoW();
        _handler = new UpdateProfileBannerHandler(_userRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_UserExists_Success() {
        // Arrange - Add a user to the in-memory repository for updating
        var user = UserFactory.InitWithDefaultValues().Build();
        await _userRepository.AddAsync(user);

        var bannerUrl = "http://example.com/new-banner.jpg";
        var command = UpdateProfileBannerCommand.Create(user.Id.Value, bannerUrl).Payload;

        // Act - Update the user's banner
        var result = await _handler.HandleAsync(command);

        // Assert - Verify the update was successful
        Assert.True(result.IsSuccess);
        var updatedUserResult = await _userRepository.GetByIdAsync(user.Id);
        Assert.Equal(bannerUrl, updatedUserResult.Payload.Profile.Banner.Url);
    }

    [Fact]
    public async Task Handle_UserDoesNotExist_Failure() {
        // Arrange - Ensure the user does not exist in the in-memory repository
        var userId = UserID.Generate().Payload.Value; // A new user ID
        var bannerUrl = "http://example.com/new-banner.jpg";
        var command = UpdateProfileBannerCommand.Create(userId, bannerUrl).Payload;

        // Act - Attempt to update a non-existing user's banner
        var result = await _handler.HandleAsync(command);

        // Assert - Verify that the update fails due to user not found
        Assert.True(result.IsFailure);
        Assert.Equal(Error.UserNotFound, result.Error);
    }
}