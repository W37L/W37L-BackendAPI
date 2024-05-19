using QueryContracts.Contracts;
using W3TL.Core.Domain.Common.Values;

namespace QueryContracts.Queries.Users;

/// <summary>
///     Represents a query to retrieve followers of a user.
/// </summary>
/// Usage:
/// var query = new GetFollowersQuery.Query(userId);
/// var answer = await queryBus.DispatchAsync(query);
/// /
public class GetFollowersQuery {
    public record Query(UserID UserID) : IQuery<Answer>;

    public record Answer(List<UserID> Users);
}