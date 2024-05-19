namespace ViaEventAssociation.Core.Domain.Common.Bases;

/// <summary>
/// Represents the base class for value objects in the domain.
/// A value object is an object that contains attributes but has no conceptual identity.
/// They should be treated as immutable.
/// </summary>
public abstract class ValueObject {
    public override bool Equals(object? obj) {
        if (obj == null || obj.GetType() != GetType())
            return false;

        ValueObject other = (ValueObject) obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>
    /// Gets the components used for equality comparison.
    /// </summary>
    /// <returns>An enumerable of objects used in equality comparison.</returns>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override int GetHashCode() {
        unchecked {
            int hash = 17;
            foreach (var obj in GetEqualityComponents()) {
                hash = hash * 23 + (obj?.GetHashCode() ?? 0);
            }

            return hash;
        }
    }

    public static bool operator ==(ValueObject? a, ValueObject? b) {
        return Equals(a, b);
    }

    public static bool operator !=(ValueObject? a, ValueObject? b) {
        return !(a == b);
    }

    public override string? ToString() {
        var components = GetEqualityComponents().Select(c => c?.ToString() ?? "null");
        return $"{GetType().Name}[{string.Join(", ", components)}]";
    }
}