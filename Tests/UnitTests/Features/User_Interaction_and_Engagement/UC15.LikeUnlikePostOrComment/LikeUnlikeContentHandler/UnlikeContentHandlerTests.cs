using UnitTests.Common.Factories;
using UnitTests.Fakes;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Application.Features.Post;

public class UnlikeContentHandlerTests {
    private readonly InMemContentRepoStub _contentRepository = new();
    private readonly UnlikeContentHandler _handler;
    private readonly InMemInteractionRepoStub _interactionRepository = new();
    private readonly FakeUoW _unitOfWork = new();
    private readonly InMemUserRepoStub _userRepository = new();

    public UnlikeContentHandlerTests() {
        _handler = new UnlikeContentHandler(_contentRepository, _unitOfWork, _userRepository, _interactionRepository);
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_UnlikesContentSuccessfully() {
        // Arrange
        var post = PostFactory.InitWithDefaultValues().Build();
        await _contentRepository.AddAsync(post);

        var unliker = UserFactory.InitWithDefaultValues().Build();
        await _userRepository.AddAsync(unliker);

        post.Like(unliker); // Like the post first

        var command = UnlikeContentCommand.Create(post.Id.Value, unliker.Id.Value).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        // Optionally, assert that the post's like count decreased or unliker was removed from likers list
    }

    [Fact]
    public async Task HandleAsync_ContentOrUserNotFound_ReturnsFailure() {
        // Arrange
        var nonExistentPostId = ValidFields.VALID_POST_ID;
        var nonExistentUnlikerId = ValidFields.VALID_USER_ID;
        var command = UnlikeContentCommand.Create(nonExistentPostId, nonExistentUnlikerId).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.PostNotFound, result.Error.EnumerateAll());
    }
}