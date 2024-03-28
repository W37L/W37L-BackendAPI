namespace ViaEventAssociation.Core.Domain.Aggregates.Guests;

/**
 * Represents a specific type of naming for last names within the domain,
 * ensuring validation and standardization as defined in the NamingType base class.
 */
public class LastNameType : NamingType {
    /**
     * Initializes a new instance of the LastNameType class with a validated name.
     * This constructor is private to ensure the name is always validated through the Create method.
     *
     * @param name The validated name value.
     */
    private LastNameType(string name) : base(name) {}

    /**
     * Creates a LastNameType instance after validating the given name string.
     *
     * @param name The name string to validate and use for creating the LastNameType instance.
     * @returns A Result containing either a LastNameType instance or an error.
     */
    public static Result<LastNameType> Create(string name) {
        var validation = Validate(name);
        if (validation.IsFailure)
            return Result<LastNameType>.Fail(validation.Error);

        return Result<LastNameType>.Ok(new LastNameType(name));
    }
}