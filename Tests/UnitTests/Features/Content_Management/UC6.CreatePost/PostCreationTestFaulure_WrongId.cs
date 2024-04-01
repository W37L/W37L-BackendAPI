using W3TL.Core.Domain.Agregates.Post.Values;

public class PostCreationTestFailure_WrongId {
    // UC6.F1 - Test for failure when PostID is blank
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreatePostID_BlankID_ReturnBlankStringError(string? postIdInput) {
        // Act
        var result = PostID.Create(postIdInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.BlankOrNullString, result.Error.EnumerateAll());
    }

    // UC6.F2 - Test for failure when PostID does not start with the required prefix
    [Theory]
    [InlineData("ID-1234567890")]
    [InlineData("XPID-1234567890123456789012345678901234567890")]
    public void CreatePostID_IncorrectPrefix_ReturnInvalidPrefixError(string? postIdInput) {
        // Act
        var result = PostID.Create(postIdInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidPrefix, result.Error.EnumerateAll());
    }

    // UC6.F3 - Test for failure when PostID does not match the expected length
    [Theory]
    [InlineData("PID-123")]
    [InlineData("PID-12345678901234567890123456789012345678901234567890")]
    public void CreatePostID_IncorrectLength_ReturnInvalidLengthError(string? postIdInput) {
        // Act
        var result = PostID.Create(postIdInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidLength, result.Error.EnumerateAll());
    }

    // UC6.F4 - Composite error test for multiple validation failures
    [Fact]
    public void CreatePostID_MultipleValidationFailures_ReturnCompositeError() {
        // Arrange
        var postIdInput = "Wrong-123"; // Example input that fails multiple validations

        // Act
        var result = PostID.Create(postIdInput);

        // Assert
        Assert.True(result.IsFailure);
        var errors = result.Error.EnumerateAll().ToList();
        Assert.Contains(Error.InvalidLength, errors);
        Assert.Contains(Error.InvalidPrefix, errors);
    }
}