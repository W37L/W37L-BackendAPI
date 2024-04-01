using UnitTests.Common.Factories;
using W3TL.Core.Domain.Agregates.Post.Values;

// Assuming a factory setup for initializing test objects

namespace UnitTests.Features.Content_Management.UC12.CreateComment;

public class CommentCreationTestFailureWrongContent {
    // UC12.F1 - Test for failure when content is blank or whitespace
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void CreateComment_BlankOrWhitespaceContent_ReturnBlankStringError(string contentInput) {
        // Arrange
        var creator = UserFactory.InitWithDefaultValues().Build();
        var signature = Signature.Create(ValidFields.VALID_SIGNATURE).Payload;
        var parentPost = PostFactory.InitWithDefaultValues().Build();

        // Act
        var contentResult = TheString.Create(contentInput);
        var commentResult = contentResult.IsSuccess
            ? Comment.Create(contentResult.Payload, creator, signature, parentPost)
            : contentResult.Error;

        // Assert
        Assert.False(commentResult.IsSuccess);
        Assert.Contains(Error.BlankOrNullString, commentResult.Error.EnumerateAll());
    }

    // UC12.F2 - Test for failure when content exceeds maximum length
    [Fact]
    public void CreateComment_TooLongContent_ReturnTooLongStringError() {
        // Arrange
        var contentInput = new string('a', TheString.MAX_LENGTH + 1); // One character beyond the max length
        var creator = UserFactory.InitWithDefaultValues().Build();
        var signature = Signature.Create(ValidFields.VALID_SIGNATURE).Payload;
        var parentPost = PostFactory.InitWithDefaultValues().Build();

        // Act
        var contentResult = TheString.Create(contentInput);
        var commentResult = contentResult.IsSuccess
            ? Comment.Create(contentResult.Payload, creator, signature, parentPost)
            : contentResult.Error;

        // Assert
        Assert.False(commentResult.IsSuccess);
        Assert.Contains(Error.TooLongString(TheString.MAX_LENGTH), commentResult.Error.EnumerateAll());
    }

    // UC12.F3 - Test for content exactly at the maximum length limit to ensure it's treated as valid
    [Fact]
    public void CreateComment_MaxLengthEdgeCase_ShouldSucceed() {
        // Arrange
        var contentInput = new string('a', TheString.MAX_LENGTH);
        var creator = UserFactory.InitWithDefaultValues().Build();
        var signature = Signature.Create(ValidFields.VALID_SIGNATURE).Payload;
        var parentPost = PostFactory.InitWithDefaultValues().Build();

        // Act
        var result = Comment.Create(TheString.Create(contentInput).Payload, creator, signature, parentPost);

        // Assert
        Assert.True(result.IsSuccess);
    }

    // UC12.F4 - Testing boundary content length that exceeds the maximum allowed length
    [Theory]
    [InlineData(141)]
    [InlineData(150)]
    [InlineData(200)]
    public void CreateComment_BoundaryContentLength_ReturnInvalidLengthError(int length) {
        // Arrange
        var contentInput = new string('a', length);
        var creator = UserFactory.InitWithDefaultValues().Build();
        var signature = Signature.Create(ValidFields.VALID_SIGNATURE).Payload;
        var parentPost = PostFactory.InitWithDefaultValues().Build();

        // Act
        var contentResult = TheString.Create(contentInput);
        var commentResult = contentResult.IsSuccess
            ? Comment.Create(contentResult.Payload, creator, signature, parentPost)
            : contentResult.Error;

        // Assert
        Assert.False(commentResult.IsSuccess);
        Assert.Contains(Error.TooLongString(TheString.MAX_LENGTH), commentResult.Error.EnumerateAll());
    }
}