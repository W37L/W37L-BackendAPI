using UnitTests.Common.Factories;
using W3TL.Core.Application.CommandDispatching.Commands.Post;

public class UpdateMediaUrlCommandTests {
    [Fact]
    public void Create_ValidArguments_ReturnsSuccess() {
        // Arrange
        var validPostId = ValidFields.VALID_POST_ID;
        var validMediaUrl = "http://example.com/media.jpg";

        // Act
        var result = UpdateMediaUrlCommand.Create(new object[] {validPostId, validMediaUrl});

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validMediaUrl, result.Payload.MediaUrl.Url);
    }

    [Fact]
    public void Create_InvalidPostId_ReturnsFailure() {
        // Arrange
        var invalidPostId = "invalid";
        var validMediaUrl = "http://example.com/media.jpg";

        // Act
        var result = UpdateMediaUrlCommand.Create(new object[] {invalidPostId, validMediaUrl});

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Create_InvalidMediaUrl_ReturnsFailure() {
        // Arrange
        var validPostId = ValidFields.VALID_POST_ID;
        var invalidMediaUrl = "not_a_valid_url";

        // Act
        var result = UpdateMediaUrlCommand.Create(new object[] {validPostId, invalidMediaUrl});

        // Assert
        Assert.False(result.IsSuccess);
    }
}