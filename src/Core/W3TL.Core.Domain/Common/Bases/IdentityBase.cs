using ViaEventAssociation.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Common.Bases;

/**
* Represents the base class for identities within the domain model.
* An identity is used to uniquely identify aggregate roots and entities.
*/
public abstract class IdentityBase : ValueObject {
    /**
 * Initializes a new instance of the IdentityBase class with a specified prefix.
 *
 * @param prefix A string prefix to prepend to the generated unique identifier.
 */
    protected IdentityBase(string prefix) {
        Value = $"{prefix}-{Guid.NewGuid()}";
    }

    /**
 * Initializes a new instance of the IdentityBase class with a specified prefix and value.
 *
 * @param prefix A string prefix to prepend to the unique identifier.
 * @param value The unique identifier value.
 */
    protected IdentityBase(string? prefix, string? value) {
        Value = value;
    }

    public string? Value { get; }

    protected override IEnumerable<object?> GetEqualityComponents() {
        yield return Value;
    }

    public override string? ToString() {
        return Value;
    }
}