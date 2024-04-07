using UnitTests.Common.Factories;
using UnitTests.Fakes;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Application.Features.Post;

public class LikeContentHandlerTests {
    private readonly InMemContentRepoStub _contentRepository = new();
    private readonly LikeContentHandler _handler;
    private readonly FakeUoW _unitOfWork = new();
    private readonly InMemUserRepoStub _userRepository = new();

    public LikeContentHandlerTests() {
        _handler = new LikeContentHandler(_contentRepository, _unitOfWork, _userRepository);
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_LikesContentSuccessfully() {
        // Arrange
        var post = PostFactory.InitWithDefaultValues().Build();
        await _contentRepository.AddAsync(post);

        var liker = UserFactory.InitWithDefaultValues().Build();
        await _userRepository.AddAsync(liker);

        var command = LikeContentCommand.Create(post.Id.Value, liker.Id.Value).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        // Optionally, assert that the post's like count increased
    }

    [Fact]
    public async Task HandleAsync_ContentOrUserNotFound_ReturnsFailure() {
        // Arrange
        var nonExistentPostId = ValidFields.VALID_POST_ID;
        var nonExistentLikerId = ValidFields.VALID_USER_ID;
        var command = LikeContentCommand.Create(nonExistentPostId, nonExistentLikerId).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        // Error should indicate the content or user was not found
    }
}