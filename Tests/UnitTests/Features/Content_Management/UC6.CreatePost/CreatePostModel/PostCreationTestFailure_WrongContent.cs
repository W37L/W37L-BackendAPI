using W3TL.Core.Domain.Agregates.Post.Values;

namespace UnitTests.Features.Content_Management.UC6.CreatePost;

public class PostCreationTestFailureWrongContent {
    // UC6.F9 - Test for failure when TheString content is blank or whitespace
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void CreateTheString_BlankOrWhitespaceContent_ReturnBlankStringError(string? contentInput) {
        // Act
        var result = TheString.Create(contentInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.BlankOrNullString, result.Error.EnumerateAll());
    }

    // UC6.F10 - Test for failure when TheString content exceeds maximum length
    [Theory]
    [InlineData("This string exceeds the maximum allowed length of one hundred and forty characters for the purpose of this test and should therefore result in a validation error, as it is too long.")]
    [InlineData("Another example of a string that is too long and exceeds the maximum length limit, causing a failure in the creation process of TheString, as it is not allowed to be longer than one hundred and forty characters.")]
    public void CreateTheString_TooLongContent_ReturnTooLongStringError(string? contentInput) {
        // Act
        var result = TheString.Create(contentInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.TooLongString(TheString.MAX_LENGTH), result.Error.EnumerateAll());
    }

    // UC6.F11 - Test for edge case: exactly at the maximum length limit
    [Fact]
    public void CreateTheString_MaxLengthEdgeCase_ShouldSucceed() {
        // Arrange
        var contentInput = new string('a', TheString.MAX_LENGTH);

        // Act
        var result = TheString.Create(contentInput);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(TheString.MAX_LENGTH, result.Payload.Value.Length);
    }

    // UC6.F12 - Test for failure when TheString boundary content is too long
    [Theory]
    [InlineData(141)]
    [InlineData(142)]
    [InlineData(234)]
    public void CreateTheString_BoundaryContentLength_ReturnInvalidLengthError(int length) {
        // Arrange
        var contentInput = new string('a', length);

        // Act
        var result = TheString.Create(contentInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.TooLongString(TheString.MAX_LENGTH), result.Error.EnumerateAll());
    }
}