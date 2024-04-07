using UnitTests.Common.Factories;
using UnitTests.Fakes;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Application.Features.Post;

public class UpdateContentHandlerTests {
    private readonly InMemContentRepoStub _contentRepository = new();
    private readonly UpdateContentHandler _handler;
    private readonly FakeUoW _unitOfWork = new();

    public UpdateContentHandlerTests() {
        _handler = new UpdateContentHandler(_contentRepository, _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_UpdatesContentSuccessfully() {
        // Arrange
        var post = PostFactory.InitWithDefaultValues().Build();
        await _contentRepository.AddAsync(post);

        var newContent = "This is the updated content";
        var signature = ValidFields.VALID_SIGNATURE;
        var command = UpdateContentCommand.Create(post.Id.Value, newContent, signature).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        var updatedPost = await _contentRepository.GetByIdAsync(post.Id);
        Assert.Equal(newContent, updatedPost.Payload.ContentTweet.Value);
    }

    [Fact]
    public async Task HandleAsync_ContentNotFound_ReturnsFailure() {
        // Arrange
        var nonExistentPostId = ValidFields.VALID_POST_ID;
        var newContent = "This is the updated content";
        var signature = ValidFields.VALID_SIGNATURE;
        var command = UpdateContentCommand.Create(nonExistentPostId, newContent, signature).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.ContentNotFound, result.Error);
    }
}