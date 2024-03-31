using W3TL.Core.Domain.Agregates.Post.Values;

namespace UnitTests.Features.Content_Management.UC6.CreatePost;

public class PostCreationTestFailure_WrongSignature {
    // UC6.F5 - Test for failure when Signature is blank
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateSignature_BlankSignature_ReturnBlankStringError(string signatureInput) {
        // Act
        var result = Signature.Create(signatureInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.BlankString, result.Error.EnumerateAll());
    }

    // UC6.F6 - Test for failure when Signature does not match hex or base64 patterns
    [Theory]
    [InlineData("ZZZ")]
    [InlineData("1234567890")]
    [InlineData("invalid-signature")]
    public void CreateSignature_InvalidFormat_ReturnInvalidSignatureError(string signatureInput) {
        // Act
        var result = Signature.Create(signatureInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidSignature, result.Error.EnumerateAll());
    }

    // UC6.F7 - Test for valid hex pattern but incorrect length
    [Theory]
    [InlineData("abcdef1234567890abcdef1234567890")] // 32 characters
    [InlineData("abcdef")] // Too short
    public void CreateSignature_HexIncorrectLength_ReturnInvalidSignatureError(string signatureInput) {
        // Act
        var result = Signature.Create(signatureInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidSignature, result.Error.EnumerateAll());
    }

    // UC6.F8 - Test for valid base64 pattern but incorrect padding
    [Theory]
    [InlineData("dGVzdA==test")] // Incorrect padding
    [InlineData("dGVzdA=test")] // Single = but not at the end
    public void CreateSignature_Base64IncorrectPadding_ReturnInvalidSignatureError(string signatureInput) {
        // Act
        var result = Signature.Create(signatureInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidSignature, result.Error.EnumerateAll());
    }

    // Additional failure scenarios specific to Signature rules can be added here
}