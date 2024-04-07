using UnitTests.Common.Factories;
using W3TL.Core.Domain.Agregates.Post.Enum;

public class CreatePostCommandTests {
    private string contentTweet = ValidFields.VALID_POST_CONTENT;
    private string creatorId = ValidFields.VALID_USER_ID;
    private string mediaType = "Video";
    private string mediaUrl = ValidFields.VALID_MEDIA_URL;

    private string postId = ValidFields.VALID_POST_ID;
    private string postType = PostType.Original.ToString();
    private string signature = ValidFields.VALID_SIGNATURE;


    [Fact]
    public void Create_WithMinimumRequiredParameters_Success() {
        // Arrange
        var args = new object[] {postId, contentTweet, creatorId, signature, postType};

        // Act
        var result = CreatePostCommand.Create(args);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(postId, result.Payload.Id.Value);
        Assert.Equal(contentTweet, result.Payload.ContentTweet.Value);
        Assert.Equal(creatorId, result.Payload.CreatorId.Value);
        Assert.Equal(signature, result.Payload.Signature.Value);
        Assert.Equal(postType, result.Payload.PostType.ToString());
    }

    [Fact]
    public void Create_WithValidMediaUrl_Success() {
        // Arrange
        var args = new object[] {postId, contentTweet, creatorId, signature, postType, mediaUrl};

        // Act
        var result = CreatePostCommand.Create(args);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(postId, result.Payload.Id.Value);
        Assert.Equal(contentTweet, result.Payload.ContentTweet.Value);
        Assert.Equal(creatorId, result.Payload.CreatorId.Value);
        Assert.Equal(signature, result.Payload.Signature.Value);
        Assert.Equal(postType, result.Payload.PostType.ToString());
        Assert.Equal(mediaUrl, result.Payload.MediaUrl.Url);
    }

    [Fact]
    public void Create_WithValidMediaType_Success() {
        // Arrange
        var args = new object[] {postId, contentTweet, creatorId, signature, postType, mediaUrl, mediaType};

        // Act
        var result = CreatePostCommand.Create(args);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(postId, result.Payload.Id.Value);
        Assert.Equal(contentTweet, result.Payload.ContentTweet.Value);
        Assert.Equal(creatorId, result.Payload.CreatorId.Value);
        Assert.Equal(signature, result.Payload.Signature.Value);
        Assert.Equal(postType, result.Payload.PostType.ToString());
        Assert.Equal(mediaUrl, result.Payload.MediaUrl.Url);
        Assert.Equal(mediaType, result.Payload.MediaType.ToString());
    }

    [Fact]
    public void Create_WithValidParentPostId_Success() {
        // Arrange
        var args = new object[] {postId, contentTweet, creatorId, signature, postType, mediaUrl, mediaType, postId};

        // Act
        var result = CreatePostCommand.Create(args);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(postId, result.Payload.Id.Value);
        Assert.Equal(contentTweet, result.Payload.ContentTweet.Value);
        Assert.Equal(creatorId, result.Payload.CreatorId.Value);
        Assert.Equal(signature, result.Payload.Signature.Value);
        Assert.Equal(postType, result.Payload.PostType.ToString());
        Assert.Equal(mediaUrl, result.Payload.MediaUrl.Url);
        Assert.Equal(mediaType, result.Payload.MediaType.ToString());
        Assert.Equal(postId, result.Payload.ParentPostId.Value);
    }

    [Fact]
    public void Create_MissingRequiredParameters_Failure() {
        // Arrange
        var args = new object[] {postId, contentTweet, creatorId, signature};

        // Act
        var result = CreatePostCommand.Create(args);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidCommand, result.Error.EnumerateAll());
    }

    [Fact]
    public void Create_InvalidPostType_Failure() {
        // Arrange
        var args = new object[] {postId, contentTweet, creatorId, signature, "InvalidPostType"};

        // Act
        var result = CreatePostCommand.Create(args);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidPostType, result.Error.EnumerateAll());
    }

    [Fact]
    public void Create_InvalidMediaUrl_Failure() {
        // Arrange
        var args = new object[] {postId, contentTweet, creatorId, signature, postType, "InvalidMediaUrl"};

        // Act
        var result = CreatePostCommand.Create(args);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidUrl, result.Error.EnumerateAll());
    }

    [Fact]
    public void Create_InvalidMediaType_Failure() {
        // Arrange
        var args = new object[] {postId, contentTweet, creatorId, signature, postType, mediaUrl, "InvalidMediaType"};

        // Act
        var result = CreatePostCommand.Create(args);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidMediaType, result.Error.EnumerateAll());
    }

    [Fact]
    public void Create_InvalidParentPostId_Failure() {
        // Arrange
        var args = new object[] {postId, contentTweet, creatorId, signature, postType, mediaUrl, mediaType, "InvalidParentPostId"};

        // Act
        var result = CreatePostCommand.Create(args);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidPrefix, result.Error.EnumerateAll());
    }

    [Fact]
    public void Create_InvalidPostId_Failure() {
        // Arrange
        var args = new object[] {"InvalidPostId", contentTweet, creatorId, signature, postType, mediaUrl, mediaType, postId};

        // Act
        var result = CreatePostCommand.Create(args);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidPrefix, result.Error.EnumerateAll());
    }
}