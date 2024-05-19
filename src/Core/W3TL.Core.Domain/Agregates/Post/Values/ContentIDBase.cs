using W3TL.Core.Domain.Common.Bases;

/// <summary>
///  Represents the base class for content
/// </summary>
public abstract class ContentIDBase : IdentityBase {
    private const int ExpectedLength = 40; // Including PREFIX + GUID length.

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentIDBase"/> class with the specified prefix.
    /// </summary>
    protected ContentIDBase(string prefix) : base(prefix) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentIDBase"/> class with the specified prefix and value.
    /// </summary>
    protected ContentIDBase(string? prefix, string? value) : base(prefix, value) { }

    /// <summary>
    /// Validates the ID value against the expected format.
    /// </summary>
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