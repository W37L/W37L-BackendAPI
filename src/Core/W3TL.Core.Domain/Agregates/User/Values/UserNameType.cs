using System.Text.RegularExpressions;
using ViaEventAssociation.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Agregates.User.Values;

/// <summary>
/// Represents a username in the system, encapsulating logic for validation
/// and instantiation to ensure usernames adhere to a specific format
/// inspired by social media platforms like Twitter or Instagram.
/// </summary>
public class UserNameType : ValueObject {
    /// <summary>
    /// Private constructor to ensure instantiation through the Create method
    /// after ensuring the username meets the required validation criteria.
    /// </summary>
    /// <param name="value">The validated username as a string.</param>
    private UserNameType(string? value) {
        this.Value = value;
    }

    public string? Value { get; }

    /// <summary>
    /// Attempts to create a UserNameType instance from a string representing the username,
    /// validating it against specific format criteria inspired by social media platforms.
    /// </summary>
    /// <param name="username">The username string to validate and use for instantiation.</param>
    /// <returns>A Result containing either a UserNameType instance or an error, based on validation outcome.</returns>
    public static Result<UserNameType> Create(string? username) {
        var validation = Validate(username);
        if (validation.IsSuccess)
            return new UserNameType(username);
        return validation.Error;
    }

    /// <summary>
    /// Validates a username string against specific criteria, including length
    /// and allowed characters, to ensure compliance with a simplified version
    /// of social media platform username standards.
    /// </summary>
    /// <param name="username">The username string to validate.</param>
    /// <returns>A Result indicating the validation outcome.</returns>
    private static Result Validate(string? username) {
        if (string.IsNullOrWhiteSpace(username))
            return Error.BlankUserName;

        // Regex pattern to enforce rules similar to Twitter/Instagram username styles.
        // Starts with an alphanumeric character and can include underscores or periods.
        // This example assumes a length of 1 to 15 characters.
        if (!Regex.IsMatch(username, @"^[a-zA-Z0-9][a-zA-Z0-9_\.]{2,14}$"))
            return Error.InvalidUserNameFormat;

        return Result.Success();
    }

    /// <summary>
    /// Provides components for equality comparison between instances of UserNameType,
    /// primarily based on the username value.
    /// </summary>
    /// <returns>An enumerable of objects used in equality comparison.</returns>
    protected override IEnumerable<object?> GetEqualityComponents() {
        yield return Value;
    }
}