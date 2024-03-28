namespace ViaEventAssociation.Core.Domain.Aggregates.Guests;

/**
 * Represents a specific type of naming for first names within the domain,
 * ensuring validation and standardization as defined in the NamingType base class.
 */
public class NameType : NamingType {
    /**
     * Initializes a new instance of the NameType class with a validated name.
     * This constructor is private to ensure the name is always validated through the Create method.
     *
     * @param name The validated name value.
     */
    private NameType(string name) : base(name) {}

    /**
     * Creates a NameType instance after validating the given name string.
     *
     * @param name The name string to validate and use for creating the NameType instance.
     * @returns A Result containing either a NameType instance or an error.
     */
    public static Result<NameType> Create(string name) {
        var validation = Validate(name);
        if (validation.IsFailure)
            return validation.Error;

        return new NameType(name);
    }
}