using QueryContracts.Contracts;
using W3TL.Core.Domain.Common.Values;

namespace QueryContracts.Queries.Users;

public class GetFollowingQuery {
    public record Query(UserID UserID) : IQuery<Answer>;

    public record Answer(List<UserID> Users);
}