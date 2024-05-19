using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Common.Values;

/// <summary>
/// Represents a timestamp value object that can be created from either a Unix timestamp
/// or a date string. This class ensures the validation of its input and provides utility
/// methods to interact with the timestamp in various formats.
/// </summary>
public class CreatedAtType : ValueObject {
    public readonly long Value;

    /// <summary>
    /// Private constructor used internally to create an instance of CreatedAtType
    /// with a validated Unix timestamp.
    /// </summary>
    /// <param name="unixTime">The Unix timestamp representing the specific point in time.</param>
    private CreatedAtType(long unixTime) {
        this.Value = unixTime;
    }

    /// <summary>
    /// Attempts to create a CreatedAtType instance from a Unix timestamp.
    /// Validates the provided Unix timestamp before instantiation.
    /// </summary>
    /// <param name="unixTime">The Unix timestamp to validate and use for creating the instance.</param>
    /// <returns>A Result containing either a CreatedAtType instance or an error.</returns>
    ///<summary>
    ///<param name="unixTime">The Unix timestamp to validate and use for creating the instance.</param>
    ///</summary>
    public static Result<CreatedAtType> Create(long unixTime) {
        var validation = ValidateUnixTime(unixTime);
        if (validation.IsSuccess)
            return new CreatedAtType(unixTime);
        return validation.Error;
    }

    ///<summary>
    ///</summary>
    public static Result<CreatedAtType> Create() {
        return new CreatedAtType(DateTimeOffset.Now.ToUnixTimeSeconds());
    }

    /// <summary>
    /// Attempts to create a CreatedAtType instance from a date string.
    /// Validates the provided date string before converting it to a Unix timestamp and instantiation.
    /// </summary>
    /// <param name="dateString">The date string to validate, convert, and use for creating the instance.</param>
    /// <returns>A Result containing either a CreatedAtType instance or an error.</returns>
    ///<summary>
    ///<param name="dateString">The date string to validate, convert, and use for creating the instance.</param>
    ///</summary>
    public static Result<CreatedAtType> Create(string dateString) {
        var validation = ValidateDateString(dateString, out long unixTime);
        if (validation.IsSuccess)
            return new CreatedAtType(unixTime);
        return validation.Error;
    }

    /// <summary>
    /// Validates a given Unix timestamp.
    /// </summary>
    /// <param name="unixTime">The Unix timestamp to validate.</param>
    /// <returns>A Result indicating the validation outcome.</returns>
    ///<summary>
    ///<param name="unixTime">The Unix timestamp to validate.</param>
    ///</summary>
    private static Result ValidateUnixTime(long unixTime) {
        if (unixTime <= 0)
            return Result.Fail(Error.InvalidUnixTime);
        return Result.Success();
    }

    /// <summary>
    /// Validates a given date string and converts it to a Unix timestamp if valid.
    /// </summary>
    /// <param name="dateString">The date string to validate and convert.</param>
    /// <param name="unixTime">The Unix timestamp converted from the date string.</param>
    /// <returns>A Result indicating the validation and conversion outcome.</returns>
    ///<summary>
    ///<param name="dateString">The date string to validate and convert.</param>
    ///<param name="unixTime">The Unix timestamp converted from the date string.</param>
    ///</summary>
    private static Result ValidateDateString(string dateString, out long unixTime) {
        unixTime = 0;
        if (string.IsNullOrWhiteSpace(dateString))
            return Error.BlankOrNullString;

        if (long.TryParse(dateString, out var parsedUnixTime)) {
            // Check if the parsed Unix time is within a reasonable range
            // Here, we assume any date from 01/01/1970 to a future date is valid
            if (parsedUnixTime < 0)
                return Error.InvalidDateFormat;

            unixTime = parsedUnixTime;
            return Result.Success();
        }

        return Error.InvalidDateFormat;
    }


    /// <summary>
    /// Converts the Unix timestamp to a DateTime object.
    /// </summary>
    /// <returns>A DateTime representation of the Unix timestamp.</returns>
    ///<summary>
    ///</summary>
    public DateTime ToDateTime() {
        return DateTimeOffset.FromUnixTimeSeconds(Value).DateTime;
    }

    /// <summary>
    /// Provides a string representation of the CreatedAtType instance in ISO 8601 format.
    /// </summary>
    /// <returns>A string representation of the CreatedAtType instance.</returns>
    ///<summary>
    ///</summary>
    public override string? ToString() {
        return ToDateTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
    }

    /// <summary>
    /// Provides the components for determining equality between CreatedAtType instances.
    /// </summary>
    /// <returns>An enumerable of objects used in equality comparison.</returns>
    ///<summary>
    ///</summary>
    protected override IEnumerable<object?> GetEqualityComponents() {
        yield return Value;
    }
}