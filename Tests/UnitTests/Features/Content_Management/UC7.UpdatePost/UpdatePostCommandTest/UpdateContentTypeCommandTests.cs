using UnitTests.Common.Factories;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Domain.Agregates.Post.Enum;
using W3TL.Core.Domain.Agregates.Post.Values;

public class UpdateContentTypeCommandTests {
    [Fact]
    public void Create_ValidArguments_ReturnsSuccess() {
        // Arrange
        var validPostId = ValidFields.VALID_POST_ID;
        var validPostType = MediaType.Text.ToString();

        // Act
        var result = UpdateContentTypeCommand.Create(new object[] {validPostId, validPostType});

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validPostId, result.Payload.Id.Value);
    }

    [Fact]
    public void Create_InvalidPostId_ReturnsFailure() {
        // Arrange
        var invalidPostId = "invalid";
        var validPostType = PostType.Original.ToString();

        // Act
        var result = UpdateContentTypeCommand.Create(new object[] {invalidPostId, validPostType});

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Create_InvalidPostType_ReturnsFailure() {
        // Arrange
        var validPostId = ValidFields.VALID_POST_ID;
        var invalidPostType = "not_a_post_type";

        // Act
        var result = UpdateContentTypeCommand.Create(new object[] {validPostId, invalidPostType});

        // Assert
        Assert.False(result.IsSuccess);
    }
}