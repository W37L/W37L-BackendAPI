namespace ViaEventAssociation.Core.Domain.Aggregates.Guests;

/// <summary>
/// Represents a specific type of naming for first names within the domain,
/// ensuring validation and standardization as defined in the NamingType base class.
/// </summary>
public class NameType : NamingType
{
    /// <summary>
    /// Initializes a new instance of the NameType class with a validated name.
    /// This constructor is private to ensure the name is always validated through the Create method.
    /// </summary>
    /// <param name="name">The validated name value.</param>
    private NameType(string? name) : base(name) { }

    /// <summary>
    /// Creates a NameType instance after validating the given name string.
    /// </summary>
    /// <param name="name">The name string to validate and use for creating the NameType instance.</param>
    /// <returns>A Result containing either a NameType instance or an error.</returns>
    public static Result<NameType> Create(string? name)
    {
        var validation = Validate(name);
        return validation.IsSuccess ? Result<NameType>.Ok(new NameType(name)) : validation.Error;
    }
}