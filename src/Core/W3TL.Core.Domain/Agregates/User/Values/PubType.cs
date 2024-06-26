using System.Text.RegularExpressions;
using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Common.Values;

/// <summary>
/// Represents a public key within the system, encapsulating the logic
/// for validation and instantiation to ensure adherence to expected formats
/// and standards, particularly those used by the NaCl library for encryption.
/// </summary>
public class PubType : ValueObject
{
    /// <summary>
    /// Private constructor to ensure instantiation through the Create method
    /// after ensuring the public key meets the required validation criteria.
    /// </summary>
    /// <param name="value">The validated public key as a string.</param>
    private PubType(string? value)
    {
        this.Value = value;
    }

    public string? Value { get; }

    /// <summary>
    /// Attempts to create a PubType instance from a string representing the public key,
    /// validating it against expected formats for NaCl library-generated keys.
    /// </summary>
    /// <param name="publicKey">The public key string to validate and use for instantiation.</param>
    /// <returns>A Result containing either a PubType instance or an error, based on validation outcome.</returns>
    public static Result<PubType> Create(string? publicKey)
    {
        var validation = Validate(publicKey);
        if (validation.IsSuccess)
            return new PubType(publicKey);
        return validation.Error;
    }

    /// <summary>
    /// Validates a public key string against specific criteria, including length
    /// and character composition, to align with keys generated by the NaCl library.
    /// </summary>
    /// <param name="publicKey">The public key string to validate.</param>
    /// <returns>A Result indicating the validation outcome.</returns>
    private static Result Validate(string? publicKey)
    {
        if (string.IsNullOrWhiteSpace(publicKey))
            return Error.BlankPublicKey;

        // NaCl public keys, when base64 encoded, are typically 44 characters including padding.
        // This regex validates a base64 encoded string of that length.
        if (!Regex.IsMatch(publicKey, @"^[a-zA-Z0-9\+/]{43}=$"))
            return Error.InvalidPublicKeyFormat;

        return Result.Success();
    }

    /// <summary>
    /// Provides components for equality comparison between instances of PubType,
    /// primarily based on the public key value.
    /// </summary>
    /// <returns>An enumerable of objects used in equality comparison.</returns>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}