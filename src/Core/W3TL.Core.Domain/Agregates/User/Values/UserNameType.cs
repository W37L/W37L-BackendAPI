using System.Text.RegularExpressions;
using ViaEventAssociation.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Agregates.User.Values;

/**
* Represents a username in the system, encapsulating logic for validation
* and instantiation to ensure usernames adhere to a specific format
* inspired by social media platforms like Twitter or Instagram.
*/
public class UserNameType : ValueObject {
    private readonly string value;

    /**
 * Private constructor to ensure instantiation through the Create method
 * after ensuring the username meets the required validation criteria.
 *
 * @param value The validated username as a string.
 */
    private UserNameType(string value) {
        this.value = value;
    }

    public string Value => this.value;

    /**
 * Attempts to create a UserNameType instance from a string representing the username,
 * validating it against specific format criteria inspired by social media platforms.
 *
 * @param username The username string to validate and use for instantiation.
 * @returns A Result containing either a UserNameType instance or an error, based on validation outcome.
 */
    public static Result<UserNameType> Create(string username) {
        var validation = Validate(username);
        if (validation.IsSuccess)
            return new UserNameType(username);
        return validation.Error;
    }

    /**
 * Validates a username string against specific criteria, including length
 * and allowed characters, to ensure compliance with a simplified version
 * of social media platform username standards.
 *
 * @param username The username string to validate.
 * @returns A Result indicating the validation outcome.
 */
    private static Result Validate(string username) {
        if (string.IsNullOrWhiteSpace(username))
            return Error.BlankUserName;

        // Regex pattern to enforce rules similar to Twitter/Instagram username styles.
        // Starts with an alphanumeric character and can include underscores or periods.
        // This example assumes a length of 1 to 15 characters.
        if (!Regex.IsMatch(username, @"^[a-zA-Z0-9][a-zA-Z0-9_\.]{2,14}$"))
            return Error.InvalidUserNameFormat;

        return Result.Success();
    }

    /**
 * Provides components for equality comparison between instances of UserNameType,
 * primarily based on the username value.
 *
 * @returns An enumerable of objects used in equality comparison.
 */
    protected override IEnumerable<object> GetEqualityComponents() {
        yield return value;
    }
}