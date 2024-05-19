using QueryContracts.Contracts;
using W3TL.Core.Domain.Common.Values;

namespace QueryContracts.Queries.Users;

/// <summary>
///     Represents a query to get the list of users being followed by a specific user.
/// </summary>
public class GetFollowingQuery {
    public record Query(UserID UserID) : IQuery<Answer>;

    public record Answer(List<UserID> Users);
}