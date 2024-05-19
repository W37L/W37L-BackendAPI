namespace ViaEventAssociation.Core.Domain.Aggregates.Guests;

/// <summary>
/// Represents a specific type of naming for last names within the domain,
/// ensuring validation and standardization as defined in the NamingType base class.
/// </summary>
public class LastNameType : NamingType {
    /// <summary>
    /// Initializes a new instance of the LastNameType class with a validated name.
    /// This constructor is private to ensure the name is always validated through the Create method.
    /// </summary>
    /// <param name="name">The validated name value.</param>
    private LastNameType(string? name) : base(name) { }

    /// <summary>
    /// Creates a LastNameType instance after validating the given name string.
    /// </summary>
    /// <param name="name">The name string to validate and use for creating the LastNameType instance.</param>
    /// <returns>A Result containing either a LastNameType instance or an error.</returns>
    public static Result<LastNameType> Create(string? name) {
        var validation = Validate(name);
        return validation.IsSuccess ? Result<LastNameType>.Ok(new LastNameType(name)) : Result<LastNameType>.Fail(validation.Error);
    }
}