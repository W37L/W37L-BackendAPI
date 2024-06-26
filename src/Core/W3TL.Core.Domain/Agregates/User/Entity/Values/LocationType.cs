using ViaEventAssociation.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Agregates.User.Entity.Values;

/// <summary>
/// Represents the location of a user.
/// </summary>
public class LocationType : ValueObject {
    public static readonly int MAX_LENGTH = 100;

    private LocationType(string? value) {
        Value = value;
    }

    /// <summary>
    /// Gets the value of the location.
    /// </summary>
    public string? Value { get; }

    /// <summary>
    /// Creates an instance of LocationType with the specified location value.
    /// </summary>
    /// <param name="value">The location value.</param>
    /// <returns>A result indicating success or failure.</returns>
    public static Result<LocationType> Create(string? value) {
        try {
            var validation = Validate(value);
            if (validation.IsFailure)
                return validation.Error;
            return new LocationType(value);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    private static Result Validate(string? value) {
        var errors = new HashSet<Error>();

        if (value is null)
            return Error.BlankOrNullString;

        if (value.Length > MAX_LENGTH)
            errors.Add(Error.TooLongLocation(MAX_LENGTH));

        if (errors.Any())
            return Error.CompileErrors(errors);

        return Result.Ok;
    }

    protected override IEnumerable<object?> GetEqualityComponents() {
        yield return Value;
    }

    public override string? ToString() {
        return Value;
    }
}