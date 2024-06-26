using ViaEventAssociation.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Agregates.Post.Values;

/// <summary>
///  Represents a string.
/// </summary>
public class TheString : ValueObject {
    public static readonly int MAX_LENGTH = 140;

    private TheString(string? value) {
        Value = value;
    }

    public string? Value { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="TheString"/> class with the specified value.
    /// </summary>
    /// <param name="value">The value of the string.</param>
    /// <returns>A result indicating success or failure with the created string.</returns>
    public static Result<TheString> Create(string? value) {
        try {
            var validation = Validate(value);
            if (validation.IsFailure)
                return validation.Error;
            return new TheString(value);
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

        if (value.Length > MAX_LENGTH)
            errors.Add(Error.TooLongString(MAX_LENGTH));

        if (errors.Any())
            return Error.CompileErrors(errors);

        return Result.Ok;
    }

    /// <summary>
    /// Returns the components used for equality comparison.
    /// </summary>
    /// <returns>The components used for equality comparison.</returns>
    protected override IEnumerable<object?> GetEqualityComponents() {
        yield return Value;
    }
}