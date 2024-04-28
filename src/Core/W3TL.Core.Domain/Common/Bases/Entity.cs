namespace W3TL.Core.Domain.Common.Bases;

/**
 * Represents the base class for all entities in the domain.
 * An entity is an object that is not defined solely by its attributes, but rather by a thread of continuity and its identity.
 *
 * @param TId The type of the identifier for the entity.
 */
public abstract class Entity<TId> where TId : IdentityBase {
    /**
     * Initializes a new instance of the Entity class with the specified identifier.
     *
     * @param id The identifier of the entity.
     */
    protected Entity(TId id) {
        Id = id;
    }

    public TId Id { get; internal set; }

    public override bool Equals(object? obj) {
        if (!(obj is Entity<TId> other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        if (Id == null || other.Id == null)
            return false;

        return Id.Equals(other.Id);
    }

    public override int GetHashCode() {
        return (GetType(), Id).GetHashCode();
    }

    public override string ToString() {
        return $"{GetType().Name}: Id: {Id}";
    }
}