using W3TL.Core.Domain.Agregates.Post.Values;

namespace UnitTests.Features.Content_Management.UC12.CreateComment;

public class CommentCreationTestFailure_WrongSignature {
    // UC12.F5 - Test for failure when Signature is blank or null
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateSignature_BlankSignature_ReturnBlankStringError(string signatureInput) {
        // Act
        var result = Signature.Create(signatureInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.BlankOrNullString, result.Error.EnumerateAll());
    }

    // UC12.F6 - Test for failure when Signature format is invalid
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

    // UC12.F7 - Test for failure when Signature hex pattern is correct but length is incorrect
    [Theory]
    [InlineData("abcdef1234567890abcdef123456")] // Incorrect length for hex
    [InlineData("abcdef")] // Too short
    public void CreateSignature_HexIncorrectLength_ReturnInvalidSignatureError(string signatureInput) {
        // Act
        var result = Signature.Create(signatureInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidSignature, result.Error.EnumerateAll());
    }

    // UC12.F8 - Test for failure when Signature base64 pattern is correct but has incorrect padding or format issues
    [Theory]
    [InlineData("dGVzdA==test")] // Incorrect padding for base64
    [InlineData("dGVzdA=test")] // Single '=' but not correctly placed
    public void CreateSignature_Base64IncorrectPadding_ReturnInvalidSignatureError(string signatureInput) {
        // Act
        var result = Signature.Create(signatureInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidSignature, result.Error.EnumerateAll());
    }
}