using UnitTests.Common.Factories;
using W3TL.Core.Application.CommandDispatching.Commands.Post;

public class UpdateContentCommandTests {
    [Fact]
    public void Create_ValidArguments_ReturnsSuccess() {
        // Arrange
        var validPostId = ValidFields.VALID_POST_ID;
        var validContentTweet = "New content tweet"; // Valid content
        var validSignature = ValidFields.VALID_SIGNATURE;

        // Act
        var result = UpdateContentCommand.Create(validPostId, validContentTweet, validSignature);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validContentTweet, result.Payload.ContentTweet.Value);
    }

    [Fact]
    public void Create_InvalidPostId_ReturnsFailure() {
        // Arrange
        var invalidPostId = "invalidPostId"; // Assuming this fails PostId creation
        var validContentTweet = "New content tweet";
        var validSignature = ValidFields.VALID_SIGNATURE;

        // Act
        var result = UpdateContentCommand.Create(invalidPostId, validContentTweet, validSignature);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.InvalidPrefix, result.Error.EnumerateAll());
    }

    [Fact]
    public void Create_InvalidContentTweet_ReturnsFailure() {
        // Arrange
        var validPostId = ValidFields.VALID_POST_ID;
        var invalidContentTweet = ""; // Assuming empty or null strings are invalid for TheString
        var validSignature = ValidFields.VALID_SIGNATURE;

        // Act
        var result = UpdateContentCommand.Create(validPostId, invalidContentTweet, validSignature);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.BlankOrNullString, result.Error.EnumerateAll());
    }

    [Fact]
    public void Create_InsufficientArguments_ReturnsFailure() {
        // Arrange
        var validPostId = ValidFields.VALID_POST_ID;

        // Act
        var result = UpdateContentCommand.Create(new object[] {validPostId});

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.InvalidCommand, result.Error.EnumerateAll());
    }
}