using ViaEventAssociation.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Common.Bases;

/**
 * Represents the base class for all aggregate roots in the domain.
 * An aggregate root is a specific type of entity that serves as the entry point to an aggregate,
 * a cluster of domain entities and value objects that are treated as a single unit.
 *
 * @param TId The type of the identifier for the aggregate root.
 */
public abstract class AggregateRoot<TId> : Entity<TId> where TId : IdentityBase {
    /**
     * Initializes a new instance of the AggregateRoot class with the specified identifier.
     *
     * @param id The identifier of the aggregate root.
     */
    protected AggregateRoot(TId id) : base(id) { }

    // Here, you could add aggregate root-specific methods and properties.
}