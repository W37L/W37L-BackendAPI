using QueryContracts.Contracts;
using W3TL.Core.Domain.Common.Values;

namespace QueryContracts.Queries.Users;

/// <summary>
///     Represents a query to get the list of users being followed by a specific user.
/// </summary>
/// Usage:
/// var query = new GetFollowingQuery.Query(userId);
/// var answer = await queryBus.DispatchAsync(query);
/// /
public class GetFollowingQuery {
    
    /// <summary>
    ///  Represents a query to get the list of users being followed by a specific user.
    /// </summary>
    /// <param name="UserId"></param>
    public record Query(UserID UserId) : IQuery<Answer>;
    
    /// <summary>
    ///  Represents the answer returned by the GetFollowingQuery.Query handler.
    /// </summary>
    /// <param name="Users"></param>
    public record Answer(List<UserID> Users);
}