using ViaEventAssociation.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Common.Bases;

/// <summary>
///     Represents the base class for identities within the domain model.
///     An identity is used to uniquely identify aggregate roots and entities.
/// </summary>
public abstract class IdentityBase : ValueObject {
    protected IdentityBase(string prefix) {
        Value = $"{prefix}-{Guid.NewGuid()}";
    }

    /// Represents the base class for identities within the domain model.
    /// An identity is used to uniquely identify aggregate roots and entities.
    /// /
    protected IdentityBase(string? prefix, string? value) {
        Value = value;
    }

    /// <summary>
    ///     Represents the base class for identities within the domain model.
    ///     An identity is used to uniquely identify aggregate roots and entities.
    /// </summary>
    public string? Value { get; internal set; }

    /// Retrieves the equality components of the IdentityBase.
    /// @return An IEnumerable of objects representing the equality components.
    /// /
    protected override IEnumerable<object?> GetEqualityComponents() {
        yield return Value;
    }

    /// Returns a string representation of the IdentityBase instance.
    /// @returns A string representing the value of the identity.
    /// /
    public override string? ToString() {
        return Value;
    }
}