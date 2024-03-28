

using ViaEventAssociation.Core.Domain.Common.Values;

public class UserCreationTestFailure_WrongCreatedAt {

    // UC1.F24 - Test for failure when CreatedAt is an invalid Unix timestamp (negative or zero)
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100000)]
    public void CreateCreatedAtType_InvalidUnixTimestamp_ReturnInvalidUnixTimeError(long unixTimeInput) {
        // Arrange

        // Act
        var result = CreatedAtType.Create(unixTimeInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidUnixTime, result.Error.EnumerateAll());
    }

    // UC1.F25 - Test for failure when CreatedAt date string is blank
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateCreatedAtType_BlankDateString_ReturnBlankStringError(string dateStringInput) {
        // Arrange

        // Act
        var result = CreatedAtType.Create(dateStringInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.BlankString, result.Error.EnumerateAll());
    }

    // UC1.F26 - Test for failure when CreatedAt date string is in an invalid format
    [Theory]
    [InlineData("20200101")]
    [InlineData("31-12-2020")]
    [InlineData("13/13/2020")]
    public void CreateCreatedAtType_InvalidDateFormatString_ReturnInvalidDateFormatError(string dateStringInput) {
        // Arrange

        // Act
        var result = CreatedAtType.Create(dateStringInput);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidDateFormat, result.Error.EnumerateAll());
    }
}
