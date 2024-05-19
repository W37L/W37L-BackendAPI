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
    
    /// <summary>
    ///  Represents a query to get the relations for a user.
    /// </summary>
    /// <param name="UserId"></param>
    public record Query(UserID UserId) : IQuery<Answer>;

    /// <summary>
    ///     Represents the answer returned by the GetRelationsQuery.Query handler.
    /// </summary>
    /// <param name="Interactions"></param>
    public record Answer(InteractionsDTO Interactions);
}