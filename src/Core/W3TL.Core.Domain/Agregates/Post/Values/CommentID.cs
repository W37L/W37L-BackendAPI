namespace W3TL.Core.Domain.Agregates.Post.Values;

public class CommentId : ContentIDBase {
    private const string Prefix = "CID";

    private CommentId(string? value) : base(Prefix, value) { }

    private CommentId(string? prefix, string? value) : base(prefix, value) { }

    public static Result<CommentId> Generate() {
        try {
            return new CommentId(Prefix);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

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