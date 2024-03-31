using ViaEventAssociation.Core.Domain.Common.Values;

namespace UnitTests.Features.User.CreateAUser;

public class UserCreationTestFailure_WrongPubKey {
    // UC1.F21 - Test for failure when PubKey is blank
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreatePubType_BlankPubKey_ReturnBlankPublicKeyError(string pubKeyInput) {
        // Arrange

        // Act
        var result = PubType.Create(pubKeyInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.BlankPublicKey, result.Error.EnumerateAll());
    }

    // UC1.F22 - Test for failure when PubKey is in incorrect format
    [Theory]
    [InlineData("ABCDEF")] // Too short
    [InlineData("12345678901234567890123456789012345678901234")] // No '=' at the end
    [InlineData("1234567890123456789012345678901234567890123?=")] // Invalid character '?'
    public void CreatePubType_InvalidFormatPubKey_ReturnInvalidPublicKeyFormatError(string pubKeyInput) {
        // Arrange

        // Act
        var result = PubType.Create(pubKeyInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidPublicKeyFormat, result.Error.EnumerateAll());
    }

    // UC1.F23 - Test for failure when PubKey does not meet the base64 format requirements
    [Theory]
    [InlineData("abc+def/ghi=jklmnopqrstuvwxyzABCDEFGHIJKLMN=")] // Valid characters but incorrect padding
    [InlineData("/+1234567890abcdefghijklmnopqrstuvwxyzABCDE==")] // Double '=' padding
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=")] // Correct length and padding, for demonstration
    public void CreatePubType_WrongBase64FormatPubKey_ReturnInvalidPublicKeyFormatError(string pubKeyInput) {
        // Arrange

        // Act
        var result = PubType.Create(pubKeyInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidPublicKeyFormat, result.Error.EnumerateAll());
    }
}