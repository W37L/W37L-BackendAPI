using W3TL.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Agregates.Post.Values;

public class PostID : IdentityBase {
    private const string PREFIX = "PID";
    private const int EXPECTED_LENGTH = 40; // Including PREFIX + GUID length.

    private PostID(string value) : base(PREFIX, value) { }

    private PostID(string prefix, string value) : base(prefix, value) { }

    public static Result<PostID> Generate() {
        try {
            return new PostID(PREFIX);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public static Result<PostID> Create(string value) {
        try {
            if (value == null) return Error.BlankString;
            var errors = new HashSet<Error>();
            if (string.IsNullOrWhiteSpace(value)) errors.Add(Error.BlankString);
            if (value.Length != EXPECTED_LENGTH) errors.Add(Error.InvalidLength);
            if (!value.StartsWith(PREFIX)) errors.Add(Error.InvalidPrefix);
            if (errors.Any()) return Error.CompileErrors(errors);
            return new PostID(value);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }



}