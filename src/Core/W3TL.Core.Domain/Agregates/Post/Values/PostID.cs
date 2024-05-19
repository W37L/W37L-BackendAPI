/// <summary>
///  Represents the unique identifier for a post.
/// </summary>
public class PostId : ContentIDBase {
    private const string Prefix = "PID";

    private PostId(string? prefix) : base(Prefix) { }

    private PostId(string? prefix, string? value) : base(prefix, value) { }

    /// <summary>
    /// Generates a new instance of the <see cref="PostId"/> class.
    /// </summary>
    /// <returns>A result indicating success or failure with the generated post ID.</returns>
    public static Result<PostId> Generate() {
        try {
            return new PostId(Prefix);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PostId"/> class with the specified value.
    /// </summary>
    /// <param name="value">The value to create the post ID from.</param>
    /// <returns>A result indicating success or failure with the created post ID.</returns>
    public static Result<PostId> Create(string? value) {
        try {
            var validation = Validate(Prefix, value);
            if (validation.IsFailure)
                return validation.Error;
            return new PostId(Prefix, value);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}