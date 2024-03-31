using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Enum;
using W3TL.Core.Domain.Agregates.Post.Values;

// Assuming this is where UserFactory is defined

namespace UnitTests.Features.Content_Management.UC6.CreatePost;

public class PostCreationTestFailureInvalidParameters {
    // UC6.F13 - Test for failure when calling Post.Create with null or invalid parameters
    [Fact]
    public void CreatePost_NullOrInvalidParameters_ReturnErrors() {
        // Arrange
        var invalidContent = ""; // Empty content is not valid
        var invalidSignature = "thisisnotavalidsignaturebecauseitisnotintheexpectedformat"; // Not a valid signature format
        global::User invalidCreator = null; // Null creator

        // Act
        var contentResult = TheString.Create(invalidContent).Payload; // This should fail due to empty content
        var signatureResult = Signature.Create(invalidSignature).Payload; // This should fail due to invalid format
        var postResult = Post.Create(contentResult, invalidCreator, signatureResult, PostType.Original);

        // Assert
        Assert.True(postResult.IsFailure, "Post creation should fail due to invalid parameters.");

        // Checking for specific errors
        var errors = postResult.Error.EnumerateAll().ToList();
        Assert.Contains(Error.NullContentTweet, errors); // Expected due to empty content
        Assert.Contains(Error.NullSignature, errors); // Expected due to invalid signature format
        Assert.Contains(Error.NullCreator, errors); // Expected due to null creator

        // This assertion checks for the presence of the NullUser error, which should be part of the error list if the creator parameter is null
    }
}