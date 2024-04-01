using W3TL.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Agregates.Post.Values;

public class PostID : IdentityBase {
    private const string? PREFIX = "PID";
    private const int EXPECTED_LENGTH = 40; // Including PREFIX + GUID length.

    private PostID(string? value) : base(PREFIX, value) { }

    private PostID(string? prefix, string? value) : base(prefix, value) { }

    public static Result<PostID> Generate() {
        try {
            return new PostID(PREFIX);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public static Result<PostID> Create(string? value) {
        try {
            var validation = Validate(value);
            if (validation.IsFailure)
                return validation.Error;
            return new PostID(value);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    private static Result Validate(string? value) {
        var errors = new HashSet<Error>();

        if (value is null)
            return Error.BlankOrNullString;

        if (string.IsNullOrWhiteSpace(value))
            errors.Add(Error.BlankOrNullString);

        if (value.Length != EXPECTED_LENGTH)
            errors.Add(Error.InvalidLength);

        if (!value.StartsWith(PREFIX))
            errors.Add(Error.InvalidPrefix);

        if (errors.Any())
            return Error.CompileErrors(errors);

        return Result.Ok;
    }
}