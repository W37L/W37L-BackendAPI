using Persistence.UserPersistence.Firebase;
using QueryContracts.Contracts;
using W3TL.Core.Domain.Common.Values;

namespace QueryContracts.Queries.Users;

/// <summary>
///     Represents a query to get the relations for a user.
/// </summary>
/// Usage:
/// var query = new GetRelationsQuery.Query(userId);
/// var answer = await queryBus.DispatchAsync(query);
/// /
public class GetRelationsQuery {
    public record Query(UserID UserID) : IQuery<Answer>;

    public record Answer(InteractionsDTO Interactions);
}