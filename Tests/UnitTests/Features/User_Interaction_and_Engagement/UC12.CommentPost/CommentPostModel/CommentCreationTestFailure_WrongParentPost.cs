using UnitTests.Common.Factories;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Values;

// Assuming you have a factory for user creation

namespace UnitTests.Features.Content_Management.UC8.CreateComment;

public class CommentCreationTestFailure_WrongParentPost {
    // UC12.F9 - Test for failure when parentPost is null
    [Fact]
    public void CreateComment_NullParentPost_ReturnNullParentPostError() {
        // Arrange
        var contentTweet = TheString.Create("Valid content").Payload;
        var creator = UserFactory.InitWithDefaultValues().Build();
        var signature = Signature.Create(ValidFields.VALID_SIGNATURE).Payload;
        Content parentPost = null;

        // Act
        var result = Comment.Create(contentTweet, creator, signature, parentPost);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.NullParentPost, result.Error.EnumerateAll());
    }
}