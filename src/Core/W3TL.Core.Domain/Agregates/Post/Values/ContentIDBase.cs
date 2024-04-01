using W3TL.Core.Domain.Common.Bases;

public abstract class ContentIDBase : IdentityBase {
    private const int ExpectedLength = 40; // Including PREFIX + GUID length.

    protected ContentIDBase(string prefix) : base(prefix) { }

    protected ContentIDBase(string? prefix, string? value) : base(prefix, value) { }

    protected static Result Validate(string prefix, string? value) {
        var errors = new HashSet<Error>();

        if (value is null)
            return Error.BlankOrNullString;

        if (string.IsNullOrWhiteSpace(value))
            errors.Add(Error.BlankOrNullString);

        if (value.Length != ExpectedLength)
            errors.Add(Error.InvalidLength);

        if (!value.StartsWith(prefix))
            errors.Add(Error.InvalidPrefix);

        if (errors.Any())
            return Error.CompileErrors(errors);

        return Result.Ok;
    }
}