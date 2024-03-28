using System.Text.RegularExpressions;
using ViaEventAssociation.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Agregates.User.Values;

/**
* Represents an email value object that encapsulates a valid email address within the domain.
* This class provides a mechanism to ensure that the email address is valid according to a general
* email format.
*/
public class EmailType : ValueObject {
    /**
 * The email value encapsulated by this EmailType instance.
 */
    public string Value { get; }

    /**
 * Initializes a new instance of the EmailType class with a given email address.
 * This constructor is private to ensure that the email address is validated before instantiation.
 *
 * @param value The validated email address.
 */
    private EmailType(string value) {
        Value = value;
    }

    /**
 * Attempts to create an EmailType instance from a given email address string, validating the input.
 *
 * @param email The email address string to validate and use for creating an EmailType instance.
 * @returns A Result containing either an EmailType instance or an error.
 */
    public static Result<EmailType> Create(string email) {
        var validation = Validate(email);
        if (validation.IsSuccess)
            return new EmailType(email);
        else
            return validation.Error;
    }

    /**
 * Validates a given email address string against a general email format.
 *
 * @param email The email address string to validate.
 * @returns A Result indicating whether the validation succeeded or failed.
 */
    private static Result Validate(string email) {
        var errors = new HashSet<Error>();

        if (email == null)
            return Error.BlankString;
        if (string.IsNullOrWhiteSpace(email))
            errors.Add(Error.BlankString);

        // General email pattern validation
        if (!Regex.IsMatch(email, @"^[\w\.-]+@[\w\.-]+\.\w+$"))
            errors.Add(Error.InvalidEmail);

        if (errors.Any())
            return Error.CompileErrors(errors);

        return Result.Success();
    }

    /**
 * Provides the components for determining equality between EmailType instances.
 * Utilizes the email address for equality comparison.
 *
 * @returns An enumerable of objects used in equality comparison.
 */
    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}