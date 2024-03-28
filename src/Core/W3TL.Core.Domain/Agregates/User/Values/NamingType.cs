
using System.Text.RegularExpressions;
using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Aggregates.Guests;

/**
 * An abstract base class designed to enforce naming conventions across different naming types within the domain.
 * This class provides a common validation mechanism for names, ensuring they meet predefined criteria for length
 * and character content. Derived classes are expected to provide specific implementations and utilization contexts,
 * such as first names, last names, etc.
 */
public abstract class NamingType : ValueObject {
    public static readonly int MAX_LENGTH = 25;
    public static readonly int MIN_LENGTH = 2;
    public string Value { get; private set; }

    /**
     * Initializes a new instance of the NamingType class with a specified name value.
     * Protected to ensure only derived classes can instantiate NamingType with a validated name.
     *
     * @param name The validated name that conforms to the specified constraints.
     */
    protected NamingType(string name) {
        Value = name;
    }

    /**
     * Validates a given name against predefined constraints, such as minimum and maximum length, and allowed characters.
     * This method ensures that names do not contain numbers or special characters, and adhere to length requirements.
     *
     * @param name The name string to validate.
     * @returns A Result indicating the validation outcome. Success if the name meets all criteria, otherwise Failure.
     */
    protected static Result Validate(string name) {
        var errors = new HashSet<Error>();

        if (string.IsNullOrEmpty(name))
            errors.Add(Error.BlankString);
        else {
            if (string.IsNullOrWhiteSpace(name))
                errors.Add(Error.BlankString);
            if (!Regex.IsMatch(name, @"^[a-zA-Z]+$"))
                errors.Add(Error.InvalidName);
            if (name.Length < MIN_LENGTH)
                errors.Add(Error.TooShortName(MIN_LENGTH));
            if (name.Length > MAX_LENGTH)
                errors.Add(Error.TooLongName(MAX_LENGTH));
        }

        if (errors.Any())
            return Result.Fail(Error.CompileErrors(errors));

        return Result.Success();
    }

    /**
     * Provides the components for determining equality between NamingType instances.
     * Utilizes the name value for equality comparison, ensuring that two instances
     * with the same name are considered equal.
     *
     * @returns An enumerable of objects used in equality comparison.
     */
    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }

}
