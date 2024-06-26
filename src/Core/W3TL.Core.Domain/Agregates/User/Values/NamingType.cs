using System.Text.RegularExpressions;
using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Aggregates.Guests;

/// <summary>
/// An abstract base class designed to enforce naming conventions across different naming types within the domain.
/// This class provides a common validation mechanism for names, ensuring they meet predefined criteria for length
/// and character content. Derived classes are expected to provide specific implementations and utilization contexts,
/// such as first names, last names, etc.
/// </summary>
public abstract class NamingType : ValueObject
{
    public static readonly int MAX_LENGTH = 25;
    public static readonly int MIN_LENGTH = 2;

    /// <summary>
    /// Initializes a new instance of the NamingType class with a specified name value.
    /// Protected to ensure only derived classes can instantiate NamingType with a validated name.
    /// </summary>
    /// <param name="name">The validated name that conforms to the specified constraints.</param>
    protected NamingType(string? name)
    {
        Value = name;
    }

    public string? Value { get; }

    /// <summary>
    /// Validates a given name against predefined constraints, such as minimum and maximum length, and allowed characters.
    /// This method ensures that names do not contain numbers or special characters, and adhere to length requirements.
    /// </summary>
    /// <param name="name">The name string to validate.</param>
    /// <returns>A Result indicating the validation outcome. Success if the name meets all criteria, otherwise Failure.</returns>
    protected static Result Validate(string? name)
    {
        var errors = new HashSet<Error>();

        if (string.IsNullOrEmpty(name))
            errors.Add(Error.BlankOrNullString);
        else
        {
            if (string.IsNullOrWhiteSpace(name))
                errors.Add(Error.BlankOrNullString);
            if (!Regex.IsMatch(name, @"^\W*[a-zA-Z]+(?: \W*[a-zA-Z]+)*\W*$"))
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

    /// <summary>
    /// Provides the components for determining equality between NamingType instances.
    /// Utilizes the name value for equality comparison, ensuring that two instances
    /// with the same name are considered equal.
    /// </summary>
    /// <returns>An enumerable of objects used in equality comparison.</returns>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}