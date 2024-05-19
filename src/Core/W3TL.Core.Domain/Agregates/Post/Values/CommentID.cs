namespace W3TL.Core.Domain.Agregates.Post.Values;

/// <summary>
/// Represents the unique identifier for a comment.
/// </summary>
public class CommentId : ContentIDBase {
    private const string Prefix = "CID";

    private CommentId(string? value) : base(Prefix) { }

    private CommentId(string? prefix, string? value) : base(prefix, value) { }

    /// <summary>
    /// Generates a new comment ID.
    /// </summary>
    public static Result<CommentId> Generate() {
        try {
            return new CommentId(Prefix);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    /// <summary>
    /// Creates a comment ID with the specified value.
    /// </summary>
    public static Result<CommentId> Create(string? value) {
        try {
            var validation = Validate(Prefix, value);
            if (validation.IsFailure)
                return validation.Error;
            return new CommentId(Prefix, value);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}