public class PostId : ContentIDBase {
    private const string Prefix = "PID";

    private PostId(string? value) : base(Prefix) { }

    private PostId(string? prefix, string? value) : base(prefix, value) { }

    public static Result<PostId> Generate() {
        try {
            return new PostId(Prefix);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

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