using System.Text.RegularExpressions;
using W3TL.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Common.Values;

/**
 * Represents a unique identifier for a User entity, extending the IdentityBase class.
 * This class allows for both the generation of new unique identifiers and the creation
 * of UserId instances from existing string values, ensuring all UserIds conform to a
 * specific format and validation rules.
 */
public class UserID : IdentityBase {
    private const string? PREFIX = "";
    private const int EXPECTED_LENGTH = 28; // Including PREFIX + GUID length.

    /**
     * Private constructor to enforce factory method usage for UserId instantiation.
     *
     * @param value The unique identifier value for the UserId, including the PREFIX.
     */
    // private UserID(string? value) : base(PREFIX) { }

    /**
     * Private constructor to enforce factory method usage for UserId instantiation.
     *
     * @param prefix The prefix for the unique identifier.
     * @param value The unique identifier value for the UserId.
     */
    private UserID(string? prefix, string? value) : base(prefix, value) { }

    /**
     * Generates a new UserId with a unique identifier prefixed with "UID-".
     * This method encapsulates the logic for creating a globally unique identifier
     * for User entities.
     *
     * @return A Result containing a new UserId instance or an error if generation fails.
     */
    // public static Result<UserID> Generate() {
    //     try {
    //         return new UserID(PREFIX);
    //     }
    //     catch (Exception exception) {
    //         return Error.FromException(exception);
    //     }
    // }

    /**
     * Validates and creates a UserId from a provided string value. This method
     * ensures the input string adheres to specific format and validation criteria,
     * such as length and prefix requirements.
     *
     * @param value The string value to validate and convert into a UserId.
     * @return A Result containing either a UserId instance or an error based on validation results.
     */
    public static Result<UserID> Create(string? value) {
        try {
            var validation = Validate(value);
            if (validation.IsFailure)
                return validation.Error;
            return new UserID(PREFIX, value);
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

        if (value.Length != EXPECTED_LENGTH)
            errors.Add(Error.InvalidLength);

        // follow the regex pattern for the prefix ^[a-zA-Z0-9]{28}$
        if (!Regex.IsMatch(value, @"^[a-zA-Z0-9]{28}$"))
            errors.Add(Error.InvalidFormat);

        if (errors.Any())
            return Error.CompileErrors(errors);

        return Result.Ok;
    }
}