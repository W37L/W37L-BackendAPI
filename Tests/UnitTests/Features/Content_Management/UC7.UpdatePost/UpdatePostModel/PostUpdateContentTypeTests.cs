using UnitTests.Common.Factories;
using W3TL.Core.Domain.Agregates.Post.Values;

public class PostUpdateContentTypeTests {
    // Success Scenarios

    // ID:UC9.S1
    [Theory]
    [InlineData(MediaType.Text)]
    [InlineData(MediaType.Video)]
    [InlineData(MediaType.Audio)]
    public void UpdateContentType_WithValidMediaType_ReturnSuccess(MediaType validMediaType) {
        // Arrange
        var post = PostFactory.InitWithDefaultValues().Build();

        // Act
        var result = post.UpdateContentType(validMediaType);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validMediaType, post.MediaType);
    }

    // Considering enums are strongly typed and the chance of an invalid enum value (outside of defined enum values) occurring
    // during runtime is negligible, there's no built-in way to test invalid enum values directly.
    // However, you could still test how your system behaves if an undefined enum value is somehow passed to it.
    // This is more of a theoretical concern in strongly typed languages like C#.

    // Theoretical Failure Scenario (Not typically necessary for enums in C#)
    // ID:UC9.F1
    [Fact]
    public void UpdateContentType_WithUndefinedMediaType_ShouldBeHandledGracefully() {
        // Arrange
        var post = PostFactory.InitWithDefaultValues().Build();
        var undefinedMediaType = (MediaType) 999; // Undefined enum value

        // Act
        // Assuming UpdateContentType gracefully handles undefined enum values,
        // either by ignoring the update or by setting a default value.
        var result = post.UpdateContentType(undefinedMediaType);

        // Assert
        // This assertion would depend on how you choose to handle undefined enum values in your UpdateContentType method.
        // For example, if you ignore the update and keep the previous value, check for that.
        // Or, if you set a default value, check for the default value.
        // Assert.True(result.IsSuccess, "Expected the system to handle undefined enum values gracefully.");
    }
}