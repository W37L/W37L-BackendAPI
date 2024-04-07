using UnitTests.Common.Factories;
using W3TL.Core.Domain.Agregates.Post.Values;

// Assuming you have this for creating comments

namespace UnitTests.Features.Content_Management.UpdateComment;

public class CommentEditTests {
    // Expanded Success Scenarios

    // ID:UC13.S1
    [Theory]
    [InlineData("Updated comment content.")]
    public void EditComment_ValidContentAndSignature_ReturnSuccess(string updatedContent) {
        // Arrange
        var comment = CommentFactory.InitWithDefaultValues().Build();
        var updatedContentTweet = TheString.Create(updatedContent).Payload;
        var updatedSignature = Signature.Create(ValidFields.VALID_SIGNATURE).Payload; // Assuming ValidFields.VALID_SIGNATURE is valid

        // Act
        var result = comment.EditContent(updatedContentTweet, updatedSignature);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(updatedContent, comment.ContentTweet.Value);
        Assert.Equal(ValidFields.VALID_SIGNATURE, comment.Signature.Value);
    }

    // Expanded Failure Scenarios

    // ID:UC13.F1
    [Theory]
    [InlineData("")]
    public void EditComment_BlankContent_ReturnFailure(string invalidContent) {
        // Arrange
        var comment = CommentFactory.InitWithDefaultValues().Build();
        var updatedSignature = Signature.Create(ValidFields.VALID_SIGNATURE).Payload;

        // Act
        var resultContent = TheString.Create(invalidContent);
        var result = comment.EditContent(resultContent.Payload, updatedSignature);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.NullContentTweet, result.Error.EnumerateAll());
    }

    // ID:UC13.F2
    [Fact]
    public void EditComment_NullSignature_ReturnFailure() {
        // Arrange
        var comment = CommentFactory.InitWithDefaultValues().Build();
        var updatedContent = TheString.Create("Valid content").Payload;
        Signature? nullSignature = null;

        // Act
        var result = comment.EditContent(updatedContent, nullSignature);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.NullSignature, result.Error.EnumerateAll());
    }

    // Add additional tests here for scenarios like null content, content too long, invalid signature format, etc.
}