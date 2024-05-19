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
    
    /// <summary>
    ///  Represents a query to get followers of a user.
    /// </summary>
    /// <param name="UserId"></param>
    public record Query(UserID UserId) : IQuery<Answer>;

    /// <summary>
    ///  Represents the answer returned by the GetFollowersQuery.Query handler.
    /// </summary>
    /// <param name="Users"></param>
    public record Answer(List<UserID> Users);
}