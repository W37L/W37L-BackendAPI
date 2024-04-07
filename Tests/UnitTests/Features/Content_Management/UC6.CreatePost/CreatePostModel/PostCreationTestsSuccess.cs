using UnitTests.Common.Factories;
using W3TL.Core.Domain.Agregates.Post.Enum;
using W3TL.Core.Domain.Agregates.Post.Values;
using static UnitTests.Common.Factories.ValidFields;

namespace UnitTests.Features.Content_Management.UC6.CreatePost;

public class PostCreationTestsSuccess {
    //ID:UC6.S1
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

    //ID:UC6.S2
    [Theory]
    [InlineData("This is a tweet")]
    [InlineData("This is a tweet with a link to a website: https://www.google.com")]
    [InlineData("This is a tweet with a link to a website: https://www.google.com and a mention to a user: @user")]
    [InlineData("This is a tweet with a mention to a user: @user")]
    public void CreatePost_WithContentTweet_ReturnSuccess(string? contentTweet) {
        //Arrange
        var creator = UserFactory.InitWithDefaultValues().Build();
        var signature = Signature.Create(VALID_SIGNATURE).Payload;
        var pType = PostType.Original;
        var contentTweetValue = TheString.Create(contentTweet).Payload;

        //Act
        var result = Post.Create(contentTweetValue, creator, signature, pType);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.Equal(contentTweet, result.Payload.ContentTweet.Value);
        Assert.Equal(creator, result.Payload.Creator);
        Assert.Equal(signature.Value, result.Payload.Signature.Value);
        Assert.Equal(pType, result.Payload.PostType);
    }

    //same test but now with values integer to check the border values
    //ID:UC6.S3
    [Theory]
    [InlineData(140)]
    [InlineData(139)]
    [InlineData(1)]
    public void CreatePost_WithContentTweetInInt_ReturnSuccess(int length) {
        //Arrange
        var contentTweet = new string('a', length);
        var creator = UserFactory.InitWithDefaultValues().Build();
        var signature = Signature.Create(VALID_SIGNATURE).Payload;
        var pType = PostType.Original;
        var contentTweetValue = TheString.Create(contentTweet).Payload;

        //Act
        var result = Post.Create(contentTweetValue, creator, signature, pType);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.Equal(contentTweet, result.Payload.ContentTweet.Value);
        Assert.Equal(creator, result.Payload.Creator);
        Assert.Equal(signature.Value, result.Payload.Signature.Value);
        Assert.Equal(pType, result.Payload.PostType);
    }

//ID:UC6.S4
    [Theory]
    [InlineData("a87bfD9E6afda16F1b9C0CdA1B8bF182Ee301Df30Fa06Dd79e0ebdAa4D137Fc6ed42aE60f7420dE6D48b548dF8CEC35E230AdbFaeF6C4fc92Da214F98a9E657c")]
    [InlineData("fdD063b2D91D47D9A0FDEfD88fA4Cf1B381Cfec2aAA83e9eD8baa04A49b31ad46631C15d3f8acbbFcFAB1DF7Cc54f3EEA8693D1223c3BeAac5e41cEAdDb7D060")]
    [InlineData("EcfF67aF788e90cEB4BfEaF0fB451cc8d7b76Ccc1E5eFBB15a6AECac07a2eEE2DB6CA3E5013DC331AF27C2b88204dd8A8FEB076Ae2F6f44d7DeeA47D247bD8a9")]
    public void CreatePost_WithValidHexSignature_ReturnSuccess(string? signatureHex) {
        //Arrange
        var contentTweet = TheString.Create("Valid content").Payload;
        var creator = UserFactory.InitWithDefaultValues().Build();
        var signature = Signature.Create(signatureHex).Payload;
        var pType = PostType.Original;

        //Act
        var result = Post.Create(contentTweet, creator, signature, pType);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(signatureHex, result.Payload.Signature.Value);
    }

    //ID:UC6.S5
    [Theory]
    [InlineData("mgINSisSslMzfLJC/8iAoRyMPLYormyyvWwyW5/bM2g/1Ms9MSRa4Mnym52+StJNl9yteB+CUONo0V7x3oLgLV==")]
    [InlineData("gTLxHothRrzYgaUPYsxQNQshEazKkpGcVFe9GvZPSsjPkZzBbdq3S8KJXBtxm/ITsp6IbpparmpkuKB8HUwobY==\n")]
    [InlineData("gTyxF+sCGN4LtkJ+b9hMEYAPCvvmZVAEFd5tgUFZGePEfMVKlGp3ze9yZxi4bUEit7/iFzwXVUWZpgmGkdfxgNt=")]
    public void CreatePost_WithValidBase64Signature_ReturnSuccess(string? signatureBase64) {
        //Arrange
        var contentTweet = TheString.Create("Valid content").Payload;
        var creator = UserFactory.InitWithDefaultValues().Build();
        var signature = Signature.Create(signatureBase64).Payload;
        var pType = PostType.Original;

        //Act
        var result = Post.Create(contentTweet, creator, signature, pType);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(signatureBase64, result.Payload.Signature.Value);
    }

    //ID:UC6.S4
    [Theory]
    [InlineData(PostType.Original)]
    [InlineData(PostType.Comment)]
    [InlineData(PostType.Retweet)]
    public void CreatePost_WithTypeVariations_ReturnSuccess(PostType pType) {
        //Arrange
        var contentTweet = TheString.Create("This is a post content.").Payload;
        var creator = UserFactory.InitWithDefaultValues().Build();
        var signature = Signature.Create("6fCA1f58Ada8fa3e219b87dEc0Cebd98C4B4ac314AD227377Ec6E581F060F46e1f0B58f7e4C23efD6fEa34eCdd6bDf6B74eB233a089AA0a67c412Cc8c7deAc0c").Payload;

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