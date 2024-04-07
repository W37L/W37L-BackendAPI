using UnitTests.Common.Factories;
using UnitTests.Fakes;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Application.Features.Post;

namespace UnitTests.Features.Content_Management.UC7.UpdatePost.UpdatePostHandlerTest;

public class UpdateContentTypeHandlerTests {
    private readonly InMemContentRepoStub _contentRepository = new InMemContentRepoStub();
    private readonly UpdateContentTypeHandler _handler;
    private readonly FakeUoW _unitOfWork = new FakeUoW();

    public UpdateContentTypeHandlerTests() {
        _handler = new UpdateContentTypeHandler(_contentRepository, _unitOfWork);
    }

    [Theory]
    [InlineData("text")]
    [InlineData("video")]
    [InlineData("audio")]
    public async void UpdateContentType_ValidContentType_ReturnSuccess(string validContentType) {
        // Arrange
        var post = PostFactory.InitWithDefaultValues().Build();
        await _contentRepository.AddAsync(post);
        var newContentCommand = UpdateContentTypeCommand.Create(post.Id, validContentType).Payload;

        // Act
        var result = await _handler.HandleAsync(newContentCommand);


        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async void UpdateContentType_PostNotFound_ReturnContentNotFound() {
        // Arrange
        var post = PostFactory.InitWithDefaultValues().Build();
        await _contentRepository.AddAsync(post);
        var newContentCommand = UpdateContentTypeCommand.Create(ValidFields.VALID_POST_ID, "text").Payload;

        // Act
        var result = await _handler.HandleAsync(newContentCommand);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.ContentNotFound, result.Error);
    }
}