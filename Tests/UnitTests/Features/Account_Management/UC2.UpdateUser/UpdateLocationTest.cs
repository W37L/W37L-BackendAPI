using UnitTests.Common.Factories;
using W3TL.Core.Domain.Agregates.User.Entity.Values;

public class UpdateLocationTest {
    // Success Scenarios

    // ID:UC2.S4
    [Theory]
    [InlineData("New York, NY")]
    [InlineData("San Francisco, CA")]
    [InlineData("")] // Assuming an empty string is a valid location.
    public void UpdateLocation_ValidInput_ShouldUpdateSuccessfully(string locationText) {
        // Arrange
        var user = UserFactory.InitWithDefaultValues().Build();

        // Act
        var locationResult = LocationType.Create(locationText);
        Assert.True(locationResult.IsSuccess);
        var result = user.Profile.UpdateLocation(locationResult.Payload);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(locationText, user.Profile.Location.Value);
    }

    // Failure Scenarios

    // ID:UC2.F12 - Null location string
    [Fact]
    public void UpdateLocation_NullLocation_ShouldFail() {
        // Arrange

        // Act
        var result = LocationType.Create(null);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.BlankString, result.Error.EnumerateAll());
    }

    // ID:UC2.F13 - Location string exceeding maximum length
    [Fact]
    public void UpdateLocation_TooLongLocation_ShouldFail() {
        // Arrange
        var longLocation = new string('a', LocationType.MAX_LENGTH + 1); // One character over the limit

        // Act
        var result = LocationType.Create(longLocation);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(Error.TooLongLocation(LocationType.MAX_LENGTH), result.Error.EnumerateAll());
    }
}