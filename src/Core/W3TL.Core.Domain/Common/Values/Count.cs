using ViaEventAssociation.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Agregates.User.Entity.Values;

/// <summary>
/// Represents a count value object, encapsulating logic for managing integer counts.
/// </summary>
public class Count : ValueObject {
    private Count(int value) {
        Value = value;
    }

    /// <summary>
    /// Gets the integer value of the count.
    /// </summary>
    public int Value { get; private set; }

    /// <summary>
    /// Gets a Count instance with a value of zero.
    /// </summary>
    public static Count Zero => new Count(0);

    /// <summary>
    /// Creates a Count instance with the specified integer value.
    /// </summary>
    /// <param name="value">The integer value of the count.</param>
    /// <returns>A Result containing either a Count instance or an error, based on validation outcome.</returns>
    public static Result<Count?> Create(int value) {
        try {
            var validation = Validate(value);
            if (validation.IsFailure)
                return validation.Error;
            return new Count(value);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    /// <summary>
    /// Creates a Count instance from a string representation of an integer value.
    /// </summary>
    /// <param name="value">The string representation of the integer value.</param>
    /// <returns>A Result containing either a Count instance or an error, based on validation outcome.</returns>
    public static Result<Count> Create(string value) {
        try {
            var validation = Validate(int.Parse(value));
            if (validation.IsFailure)
                return validation.Error;
            return new Count(int.Parse(value));
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    /// <summary>
    /// Increments the count value by one.
    /// </summary>
    /// <returns>A Result indicating the outcome of the operation.</returns>
    public Result Increment() {
        try {
            Value++;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    /// <summary>
    /// Decrements the count value by one.
    /// </summary>
    /// <returns>A Result indicating the outcome of the operation.</returns>
    public Result Decrement() {
        try {
            Value--;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    /// <summary>
    /// Validates the count value to ensure it is not negative.
    /// </summary>
    /// <param name="value">The count value to validate.</param>
    /// <returns>A Result indicating the outcome of the validation.</returns>
    private static Result Validate(int value) {
        var errors = new HashSet<Error>();

        if (value < 0)
            errors.Add(Error.NegativeValue);

        if (errors.Any())
            return Error.CompileErrors(errors);

        return Result.Ok;
    }

    /// <summary>
    /// Provides the components for determining equality between Count instances.
    /// </summary>
    /// <returns>An enumerable of objects used in equality comparison.</returns>
    protected override IEnumerable<object?> GetEqualityComponents() {
        throw new NotImplementedException();
    }
}