using UnitTests.Common.Factories;
using W3TL.Core.Application.CommandDispatching.Commands.Post;

public class UpdateContentCommandTests {
    [Fact]
    public void Create_ValidArguments_ReturnsSuccess() {
        // Arrange
        var validPostId = ValidFields.VALID_POST_ID;
        var validContentTweet = "New content tweet"; // Valid content

        // Act
        var result = UpdateContentCommand.Create(new object[] {validPostId, validContentTweet});

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validContentTweet, result.Payload.ContentTweet.Value);
    }

    [Fact]
    public void Create_InvalidPostId_ReturnsFailure() {
        // Arrange
        var invalidPostId = "invalidPostId"; // Assuming this fails PostId creation
        var validContentTweet = "New content tweet";

        // Act
        var result = UpdateContentCommand.Create(new object[] {invalidPostId, validContentTweet});

        // Assert
        Assert.False(result.IsSuccess);
        // Optionally, assert specific errors are present in the result
    }

    [Fact]
    public void Create_InvalidContentTweet_ReturnsFailure() {
        // Arrange
        var validPostId = ValidFields.VALID_POST_ID;
        var invalidContentTweet = ""; // Assuming empty or null strings are invalid for TheString

        // Act
        var result = UpdateContentCommand.Create(new object[] {validPostId, invalidContentTweet});

        // Assert
        Assert.False(result.IsSuccess);
        // Optionally, assert specific errors are present in the result
    }

    [Fact]
    public void Create_InsufficientArguments_ReturnsFailure() {
        // Arrange
        var validPostId = ValidFields.VALID_POST_ID;

        // Act
        var result = UpdateContentCommand.Create(new object[] {validPostId});

        // Assert
        Assert.False(result.IsSuccess);
        // Optionally, assert that the specific error returned is related to insufficient arguments
    }
}