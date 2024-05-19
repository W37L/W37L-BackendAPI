using System.Text.RegularExpressions;
using ViaEventAssociation.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Agregates.User.Values;

/// <summary>
/// Represents an email value object that encapsulates a valid email address within the domain.
/// This class provides a mechanism to ensure that the email address is valid according to a general
/// email format.
/// </summary>
public class EmailType : ValueObject {

    /// <summary>
    /// Initializes a new instance of the EmailType class with a given email address.
    /// This constructor is private to ensure that the email address is validated before instantiation.
    /// </summary>
    /// <param name="value">The validated email address.</param>
    private EmailType(string? value) {
        Value = value;
    }

    /// <summary>
    /// The email value encapsulated by this EmailType instance.
    /// </summary>
    public string? Value { get; }

    /// <summary>
    /// Attempts to create an EmailType instance from a given email address string, validating the input.
    /// </summary>
    /// <param name="email">The email address string to validate and use for creating an EmailType instance.</param>
    /// <returns>A Result containing either an EmailType instance or an error.</returns>
    public static Result<EmailType> Create(string? email) {
        var validation = Validate(email);
        if (validation.IsSuccess)
            return new EmailType(email);
        else
            return validation.Error;
    }

    /// <summary>
    /// Validates a given email address string against a general email format.
    /// </summary>
    /// <param name="email">The email address string to validate.</param>
    /// <returns>A Result indicating whether the validation succeeded or failed.</returns>
    private static Result Validate(string? email) {
        var errors = new HashSet<Error>();

        if (email == null)
            return Error.BlankOrNullString;
        if (string.IsNullOrWhiteSpace(email))
            errors.Add(Error.BlankOrNullString);

        // General email pattern validation
        if (!Regex.IsMatch(email, @"^[\w\.-]+@[\w\.-]+\.\w+$"))
            errors.Add(Error.InvalidEmail);

        if (errors.Any())
            return Error.CompileErrors(errors);

        return Result.Success();
    }

    /// <summary>
    /// Provides the components for determining equality between EmailType instances.
    /// Utilizes the email address for equality comparison.
    /// </summary>
    /// <returns>An enumerable of objects used in equality comparison.</returns>
    protected override IEnumerable<object?> GetEqualityComponents() {
        yield return Value;
    }
}