using UnitTests.Common.Factories;
using UnitTests.Fakes;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Application.Features.Post;
using W3TL.Core.Domain.Agregates.Post.Values;

public class UpdateMediaUrlHandlerTests {
    private readonly InMemContentRepoStub _contentRepository = new InMemContentRepoStub();
    private readonly UpdateMediaUrlHandler _handler;
    private readonly FakeUoW _unitOfWork = new FakeUoW();

    public UpdateMediaUrlHandlerTests() {
        _handler = new UpdateMediaUrlHandler(_contentRepository, _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_UpdatesMediaUrlSuccessfully() {
        // Arrange
        var post = PostFactory.InitWithDefaultValues().Build();
        await _contentRepository.AddAsync(post);

        var newMediaUrl = MediaUrl.Create("www.example.com/new-media.jpg").Payload.Url;
        var command = UpdateMediaUrlCommand.Create(post.Id, newMediaUrl).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newMediaUrl, post.MediaUrl.Url);
    }

    [Fact]
    public async Task HandleAsync_PostNotFound_ReturnsContentNotFound() {
        // Arrange
        var post = PostFactory.InitWithDefaultValues().Build();
        await _contentRepository.AddAsync(post);

        var newMediaUrl = MediaUrl.Create("www.example.com/new-media.jpg").Payload.Url;
        var command = UpdateMediaUrlCommand.Create(ValidFields.VALID_POST_ID, newMediaUrl).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.ContentNotFound, result.Error);
    }
}