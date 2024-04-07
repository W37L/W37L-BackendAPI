using UnitTests.Common.Factories;
using UnitTests.Fakes;
using W3TL.Core.Application.Features.Post;
using W3TL.Core.Domain.Agregates.Post.Enum;
using W3TL.Core.Domain.Agregates.Post.Values;

public class CreatePostHandlerTests {
    private readonly InMemContentRepoStub _contentRepository;
    private readonly CreatePostHandler _handler;
    private readonly FakeUoW _unitOfWork;
    private readonly InMemUserRepoStub _userRepository;

    public CreatePostHandlerTests() {
        _contentRepository = new InMemContentRepoStub();
        _userRepository = new InMemUserRepoStub();
        _unitOfWork = new FakeUoW();
        _handler = new CreatePostHandler(_contentRepository, _unitOfWork, _userRepository);
    }

    [Fact]
    public async Task HandleAsync_CreatesPostSuccessfully() {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();
        await _userRepository.AddAsync(user);

        var command = CreatePostCommand.Create(
            PostId.Generate().Payload.Value,
            ValidFields.VALID_POST_CONTENT,
            user.Id.Value,
            ValidFields.VALID_SIGNATURE,
            PostType.Original.ToString(),
            ValidFields.VALID_MEDIA_URL,
            MediaType.Text.ToString(),
            null // Assuming no parent post
        ).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        // Further asserts can be made to check if the post has been added correctly to the repository
    }

    [Fact]
    public async Task HandleAsync_UserNotFound_Failure() {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();

        var command = CreatePostCommand.Create(
            PostId.Generate().Payload.Value,
            ValidFields.VALID_POST_CONTENT,
            user.Id.Value,
            ValidFields.VALID_SIGNATURE,
            PostType.Original.ToString(),
            ValidFields.VALID_MEDIA_URL,
            MediaType.Text.ToString(),
            null // Assuming no parent post
        ).Payload;

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.UserNotFound, result.Error);
    }
}