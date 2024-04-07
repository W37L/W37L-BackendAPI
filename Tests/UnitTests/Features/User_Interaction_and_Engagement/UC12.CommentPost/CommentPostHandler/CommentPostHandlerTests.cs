using UnitTests.Common.Factories;
using UnitTests.Fakes;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Application.Features.User;

public class CommentPostHandlerTests {
    private readonly InMemContentRepoStub _contentRepository = new();
    private readonly CommentPostHandler _handler;
    private readonly FakeUoW _unitOfWork = new();
    private readonly InMemUserRepoStub _userRepository = new();

    public CommentPostHandlerTests() {
        _handler = new CommentPostHandler(_contentRepository, _unitOfWork, _userRepository);
    }

    [Fact]
    public async Task HandleAsync_ValidComment_CreatesSuccessfully() {
        // Arrange
        var post = PostFactory.InitWithDefaultValues().Build();
        await _contentRepository.AddAsync(post);

        var user = UserFactory.InitWithDefaultValues().Build();
        await _userRepository.AddAsync(user);

        var command = CommentPostCommand.Create(post.Id.Value, "New comment", user.Id.Value, ValidFields.VALID_SIGNATURE, post.Id.Value).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(post.Comments);
    }

    [Fact]
    public async Task HandleAsync_UserOrPostNotFound_ReturnsFailure() {
        // Arrange
        var nonExistentPostId = PostFactory.InitWithDefaultValues().Build().Id; // Not added to repository
        var nonExistentUserId = UserFactory.InitWithDefaultValues().Build().Id; // Not added to repository

        var command = CommentPostCommand.Create(nonExistentPostId.Value, "New comment", nonExistentUserId.Value, ValidFields.VALID_SIGNATURE, nonExistentPostId.Value).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.ParentPostNotFound, result.Error.EnumerateAll());
    }
}