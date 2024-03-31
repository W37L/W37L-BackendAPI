using W3TL.Core.Domain.Common.Values;

public class UserCreationTestFailure_WrongId {
    //UC1.F1
    // Test for failure when UserID is blank
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateUser_BlankUserID_ReturnBlankStringError(string userIdInput) {
        // Arrange - setup for the test; in this case, input preparation.

        // Act - the action or method under test.
        var result = UserID.Create(userIdInput);

        // Assert - verifying the outcome.
        Assert.True(result.IsFailure);
        Assert.Contains(Error.BlankString, result.Error.EnumerateAll());
    }

    //UC1.F2
    // Test for failure when UserID does not start with the required prefix
    [Theory]
    [InlineData("ID-123456789012345678901234567890123456")]
    [InlineData("XUID-123456789012345678901234567890123456")]
    public void CreateUser_IncorrectPrefixUserID_ReturnInvalidPrefixError(string userIdInput) {
        // Arrange

        // Act
        var result = UserID.Create(userIdInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidPrefix, result.Error.EnumerateAll());
    }

    //UC1.F3
    // Test for failure when UserID does not match the expected length
    [Theory]
    [InlineData("UID-123")]
    [InlineData("UID-123456789012345678901234567890123456789012345")]
    public void CreateUser_IncorrectLengthUserID_ReturnInvalidLengthError(string userIdInput) {
        // Arrange

        // Act
        var result = UserID.Create(userIdInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidLength, result.Error.EnumerateAll());
    }

    //UC1.F4
    // Example demonstrating how to structure a failure test case for multiple validation failures
    [Fact]
    public void CreateUser_MultipleValidationFailures_ReturnCompositeError() {
        // Arrange
        var userIdInput = "Wrong-123"; // Example input that fails multiple validations

        // Act
        var result = UserID.Create(userIdInput);

        // Assert - Assuming Error.CompileErrors aggregates errors into a list that GetAllErrors can retrieve
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidLength, result.Error.EnumerateAll());
        Assert.Contains(Error.InvalidPrefix, result.Error.EnumerateAll());
    }
}