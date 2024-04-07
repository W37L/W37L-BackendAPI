using UnitTests.Common.Factories;
using UnitTests.Fakes;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Application.Features.Post;

namespace UnitTests.Features.Content_Management.UC7.UpdatePost.UpdatePostHandlerTest;

public class UpdateContentHandlerTests {
    private readonly InMemContentRepoStub _contentRepository = new InMemContentRepoStub();
    private readonly UpdateContentHandler _handler;
    private readonly FakeUoW _unitOfWork = new FakeUoW();

    public UpdateContentHandlerTests() {
        _handler = new UpdateContentHandler(_contentRepository, _unitOfWork);
    }

    [Fact]
    public async void Create_ValidArguments_ReturnsSuccess() {
        // Arrange
        var post = PostFactory.InitWithDefaultValues().Build();
        await _contentRepository.AddAsync(post);
        var newContent = "new content";
        var signature = ValidFields.VALID_SIGNATURE;
        var command = UpdateContentCommand.Create(post.Id, newContent, signature).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newContent, post.ContentTweet.Value);
    }

    [Fact]
    public async void Create_PostNotFound_ReturnsContentNotFound() {
        // Arrange
        var post = PostFactory.InitWithDefaultValues().Build();
        await _contentRepository.AddAsync(post);
        var newContent = "new content";
        var signature = ValidFields.VALID_SIGNATURE;
        var command = UpdateContentCommand.Create(ValidFields.VALID_POST_ID, newContent, signature!).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.ContentNotFound, result.Error);
    }
}