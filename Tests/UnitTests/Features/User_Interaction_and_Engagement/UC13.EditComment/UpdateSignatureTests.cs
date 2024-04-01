using UnitTests.Common.Factories;
using W3TL.Core.Domain.Agregates.Post.Values;

namespace UnitTests.Features.Content_Management.UpdateComment;

public class UpdateSignatureTests {
    // Test for updating only the signature of a comment
    // ID:U13.S1
    [Fact]
    public void UpdateSignature_ValidSignature_ReturnSuccess() {
        // Arrange
        var comment = CommentFactory.InitWithDefaultValues().Build();
        var validSignature = Signature.Create(ValidFields.VALID_SIGNATURE).Payload;

        // Assuming EditComment method allows updating signature only
        var updatedContent = TheString.Create("Existing content").Payload; // Keep existing content unchanged

        // Act
        var result = comment.EditContent(updatedContent, validSignature);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ValidFields.VALID_SIGNATURE, comment.Signature.Value);
    }

    // ID:U13.F1
    [Theory]
    [InlineData("invalid-signature-format")]
    public void UpdateSignature_InvalidFormat_ReturnFailure(string invalidSignature) {
        // Arrange
        var comment = CommentFactory.InitWithDefaultValues().Build();
        var invalidSignatureResult = Signature.Create(invalidSignature);

        // Act
        Assert.True(invalidSignatureResult.IsFailure);

        // Assert
        Assert.Contains(Error.InvalidSignature, invalidSignatureResult.Error.EnumerateAll());
    }
}