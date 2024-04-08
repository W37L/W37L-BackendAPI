using Microsoft.Extensions.DependencyInjection;
using UnitTests.Common.Factories;
using UnitTests.Fakes.Handlers;
using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using W3TL.Core.Domain.Agregates.Post.Enum;

namespace UnitTests.DispatcherTest;

public class DispatcherInteractionTest {
    private readonly ICommandDispatcher _commandDispatcher;

    private readonly ServiceProvider _serviceProvider;

    public DispatcherInteractionTest() {
        // Set up the DI container
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ICommandHandler<CommentPostCommand>, GenericFakeHandler<CommentPostCommand>.CommentPostFakeHandler>();
        serviceCollection.AddScoped<ICommandHandler<CreatePostCommand>, GenericFakeHandler<CreatePostCommand>.CreatePostFakeHandler>();
        serviceCollection.AddScoped<ICommandHandler<LikeContentCommand>, GenericFakeHandler<LikeContentCommand>.LikeContentFakeHandler>();
        serviceCollection.AddScoped<ICommandHandler<UnlikeContentCommand>, GenericFakeHandler<UnlikeContentCommand>.UnlikeContentFakeHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateContentCommand>, GenericFakeHandler<UpdateContentCommand>.UpdateContentFakeHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateContentTypeCommand>, GenericFakeHandler<UpdateContentTypeCommand>.UpdateContentTypeFakeHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateMediaUrlCommand>, GenericFakeHandler<UpdateMediaUrlCommand>.UpdateMediaUrlFakeHandler>();

        _serviceProvider = serviceCollection.BuildServiceProvider();
        _commandDispatcher = new CommandDispatcher(_serviceProvider);
    }

    [Fact]
    public async Task Dispatch_CommentPostCommand_HandlerIsCalled() {
        // Arrange
        var postId = PostFactory.InitWithDefaultValues().Build().Id.Value;
        var content = ValidFields.VALID_POST_CONTENT;
        var creatorId = UserFactory.InitWithDefaultValues().Build().Id.Value;
        var signature = ValidFields.VALID_SIGNATURE;
        var parentPostId = postId; // Assuming the comment is on the original post itself for simplicity

        // Act
        var command = CommentPostCommand.Create(postId, content, creatorId, signature, parentPostId).Payload;
        // Act
        await _commandDispatcher.DispatchAsync(command);

        // Assert
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<CommentPostCommand>>() as GenericFakeHandler<CommentPostCommand>.CommentPostFakeHandler;
        Assert.NotNull(handler);
        Assert.True(handler.WasCalled);
        Assert.Equal(command, handler.HandledCommand);
    }

    [Fact]
    public async Task Dispatch_CreatePostCommand_HandlerIsNotCalled() {
        var postId = PostFactory.InitWithDefaultValues().Build().Id.Value;
        var content = ValidFields.VALID_POST_CONTENT;
        var creatorId = UserFactory.InitWithDefaultValues().Build().Id.Value;
        var signature = ValidFields.VALID_SIGNATURE;
        var postType = PostType.Original;

        // Act
        var command = CreatePostCommand.Create(postId, content, creatorId, signature, postType).Payload;
        // Act
        await _commandDispatcher.DispatchAsync(command);

        // Assert
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<CommentPostCommand>>() as GenericFakeHandler<CommentPostCommand>.CommentPostFakeHandler;
        Assert.NotNull(handler);
        Assert.False(handler.WasCalled);
    }

    [Fact]
    public async Task Dispatch_CreatePostCommand_HandlerIsCalled() {
        // Arrange
        var postId = PostFactory.InitWithDefaultValues().Build().Id.Value;
        var content = ValidFields.VALID_POST_CONTENT;
        var creatorId = UserFactory.InitWithDefaultValues().Build().Id.Value;
        var signature = ValidFields.VALID_SIGNATURE;
        var postType = PostType.Original;

        // Act
        var command = CreatePostCommand.Create(postId, content, creatorId, signature, postType).Payload;
        // Act
        await _commandDispatcher.DispatchAsync(command);

        // Assert
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<CreatePostCommand>>() as GenericFakeHandler<CreatePostCommand>.CreatePostFakeHandler;
        Assert.NotNull(handler);
        Assert.True(handler.WasCalled);
        Assert.Equal(command, handler.HandledCommand);
    }

    [Fact]
    public async Task Dispatch_LikeContentCommand_HandlerIsCalled() {
        // Arrange
        var contentId = ValidFields.VALID_POST_ID;
        var userId = UserFactory.InitWithDefaultValues().Build().Id.Value;

        // Act
        var command = LikeContentCommand.Create(contentId, userId).Payload;
        // Act
        await _commandDispatcher.DispatchAsync(command);

        // Assert
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<LikeContentCommand>>() as GenericFakeHandler<LikeContentCommand>.LikeContentFakeHandler;
        Assert.NotNull(handler);
        Assert.True(handler.WasCalled);
        Assert.Equal(command, handler.HandledCommand);
    }

    [Fact]
    public async Task Dispatch_UnlikeContentCommand_HandlerIsCalled() {
        // Arrange
        var contentId = ValidFields.VALID_POST_ID;
        var userId = UserFactory.InitWithDefaultValues().Build().Id.Value;

        // Act
        var command = UnlikeContentCommand.Create(contentId, userId).Payload;
        // Act
        await _commandDispatcher.DispatchAsync(command);

        // Assert
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<UnlikeContentCommand>>() as GenericFakeHandler<UnlikeContentCommand>.UnlikeContentFakeHandler;
        Assert.NotNull(handler);
        Assert.True(handler.WasCalled);
        Assert.Equal(command, handler.HandledCommand);
    }

    [Fact]
    public async Task Dispatch_UpdateContentCommand_HandlerIsCalled() {
        // Arrange
        var contentId = ValidFields.VALID_POST_ID;
        var content = ValidFields.VALID_POST_CONTENT;
        var signature = ValidFields.VALID_SIGNATURE;

        // Act
        var command = UpdateContentCommand.Create(contentId, content, signature).Payload;
        // Act
        await _commandDispatcher.DispatchAsync(command);

        // Assert
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<UpdateContentCommand>>() as GenericFakeHandler<UpdateContentCommand>.UpdateContentFakeHandler;
        Assert.NotNull(handler);
        Assert.True(handler.WasCalled);
        Assert.Equal(command, handler.HandledCommand);
    }

    [Fact]
    public async Task Dispatch_UpdateContentTypeCommand_HandlerIsCalled() {
        // Arrange
        var contentId = ValidFields.VALID_POST_ID;
        var contentType = ValidFields.VALID_CONTENT_TYPE;

        // Act
        var command = UpdateContentTypeCommand.Create(contentId, contentType).Payload;
        // Act
        await _commandDispatcher.DispatchAsync(command);

        // Assert
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<UpdateContentTypeCommand>>() as GenericFakeHandler<UpdateContentTypeCommand>.UpdateContentTypeFakeHandler;
        Assert.NotNull(handler);
        Assert.True(handler.WasCalled);
        Assert.Equal(command, handler.HandledCommand);
    }

    [Fact]
    public async Task Dispatch_UpdateMediaUrlCommand_HandlerIsCalled() {
        // Arrange
        var contentId = ValidFields.VALID_POST_ID;
        var mediaUrl = ValidFields.VALID_MEDIA_URL;

        // Act
        var command = UpdateMediaUrlCommand.Create(contentId, mediaUrl).Payload;
        // Act
        await _commandDispatcher.DispatchAsync(command);

        // Assert
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<UpdateMediaUrlCommand>>() as GenericFakeHandler<UpdateMediaUrlCommand>.UpdateMediaUrlFakeHandler;
        Assert.NotNull(handler);
        Assert.True(handler.WasCalled);
        Assert.Equal(command, handler.HandledCommand);
    }
}