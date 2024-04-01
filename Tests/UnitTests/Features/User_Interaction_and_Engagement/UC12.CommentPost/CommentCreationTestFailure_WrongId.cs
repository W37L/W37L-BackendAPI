using W3TL.Core.Domain.Agregates.Post.Values;

namespace UnitTests.Features.Content_Management.UC8.CreateComment;

public class CommentCreationTestFailure_WrongId {
    // UC8.F1 - Test for failure when CommentID is blank
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateCommentID_BlankID_ReturnBlankStringError(string? commentIdInput) {
        // Act
        var result = CommentId.Create(commentIdInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.BlankOrNullString, result.Error.EnumerateAll());
    }

    // UC8.F2 - Test for failure when CommentID does not start with the required prefix
    [Theory]
    [InlineData("ID-1234567890")]
    [InlineData("XCID-1234567890123456789012345678901234567890")]
    public void CreateCommentID_IncorrectPrefix_ReturnInvalidPrefixError(string? commentIdInput) {
        // Act
        var result = CommentId.Create(commentIdInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidPrefix, result.Error.EnumerateAll());
    }

    // UC8.F3 - Test for failure when CommentID does not match the expected length
    [Theory]
    [InlineData("CID-123")]
    [InlineData("CID-12345678901234567890123456789012345678901234567890")]
    public void CreateCommentID_IncorrectLength_ReturnInvalidLengthError(string? commentIdInput) {
        // Act
        var result = CommentId.Create(commentIdInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidLength, result.Error.EnumerateAll());
    }

    // UC8.F4 - Composite error test for multiple validation failures
    [Fact]
    public void CreateCommentID_MultipleValidationFailures_ReturnCompositeError() {
        // Arrange
        var commentIdInput = "Wrong-123"; // Example input that fails multiple validations

        // Act
        var result = CommentId.Create(commentIdInput);

        // Assert
        Assert.True(result.IsFailure);
        var errors = result.Error.EnumerateAll().ToList();
        Assert.Contains(Error.InvalidLength, errors);
        Assert.Contains(Error.InvalidPrefix, errors);
    }
}