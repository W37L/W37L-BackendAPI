using System.Text.RegularExpressions;
using W3TL.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Common.Values;

/// <summary>
///     Represents a unique identifier for a User entity, extending the IdentityBase class.
///     This class allows for both the generation of new unique identifiers and the creation
///     of UserId instances from existing string values, ensuring all UserIds conform to a
///     specific format and validation rules.
/// </summary>
public class UserID : IdentityBase {
    private const string?
        PREFIX = ""; // Prefix for the unique identifier, since we are using firebase, we don't need a prefix

    private const int EXPECTED_LENGTH = 28; // Including PREFIX + GUID length.


    /// <summary>
    ///     Private constructor to enforce factory method usage for UserId instantiation.
    /// </summary>
    /// <param name="prefix">The prefix for the unique identifier.</param>
    /// <param name="value">The unique identifier value for the UserId.</param>
    private UserID(string? prefix, string? value) : base(prefix, value) { }

    /// <summary>
    ///     Generates a new UserId with a unique identifier prefixed with "UID-".
    ///     This method encapsulates the logic for creating a globally unique identifier
    ///     for User entities.
    /// </summary>
    /// <returns>A Result containing a new UserId instance or an error if generation fails.</returns>
    public static Result<UserID> Generate() {
        var rawGuid = Guid.NewGuid().ToString("N"); // Remove dashes
        var trimmedGuid = rawGuid.Substring(0, EXPECTED_LENGTH - PREFIX.Length);
        var fullUserId = PREFIX + trimmedGuid;
        return new UserID(PREFIX, fullUserId);
    }

    /// <summary>
    ///     Validates and creates a UserId from a provided string value. This method
    ///     ensures the input string adheres to specific format and validation criteria,
    ///     such as length and prefix requirements.
    /// </summary>
    /// <param name="value">The string value to validate and convert into a UserId.</param>
    /// <returns>A Result containing either a UserId instance or an error based on validation results.</returns>
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

    /// <summary>
    ///     Validates a provided string value against the required format and length specifications.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <returns>A Result indicating success or containing errors if validation fails.</returns>
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