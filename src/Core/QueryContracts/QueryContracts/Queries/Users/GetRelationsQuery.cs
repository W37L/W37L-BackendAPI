using Persistence.UserPersistence.Firebase;
using QueryContracts.Contracts;
using W3TL.Core.Domain.Common.Values;

namespace QueryContracts.Queries.Users;

/// <summary>
///     Represents a query to get the relations for a user.
/// </summary>
public class GetRelationsQuery {
    public record Query(UserID UserID) : IQuery<Answer>;

    public record Answer(InteractionsDTO Interactions);
}