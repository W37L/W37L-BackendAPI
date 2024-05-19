using ViaEventAssociation.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Common.Bases;

/// <summary>
/// Represents the base class for all aggregate roots in the domain.
/// An aggregate root is a specific type of entity that serves as the entry point to an aggregate,
/// a cluster of domain entities and value objects that are treated as a single unit.
/// </summary>
/// <typeparam name="TId">The type of the identifier for the aggregate root.</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId> where TId : IdentityBase {
    
    /// <summary>
    /// Initializes a new instance of the AggregateRoot class with the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the aggregate root.</param>
    protected AggregateRoot(TId id) : base(id) { }
}