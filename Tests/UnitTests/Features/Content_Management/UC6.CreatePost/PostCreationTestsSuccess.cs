using UnitTests.Common.Factories;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Enum;
using W3TL.Core.Domain.Agregates.Post.Values;
using static UnitTests.Common.Factories.ValidFields;

public class PostCreationTestsSuccess {
    [Fact]
    public void CreatePost_WithAllValidFields_ReturnSuccess() {
        //Arrange
        var contentTweet = TheString.Create(VALID_POST_CONTENT).Payload;
        var creator = UserFactory.InitWithDefaultValues().Build();
        var signature = Signature.Create(VALID_SIGNATURE).Payload;
        var pType = PostType.Original;

        //Act
        var result = Post.Create(contentTweet, creator, signature, pType);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.Equal(contentTweet.Value, result.Payload.ContentTweet.Value);
        Assert.Equal(creator, result.Payload.Creator);
        Assert.Equal(signature.Value, result.Payload.Signature.Value);
        Assert.Equal(pType, result.Payload.PostType);
    }
}