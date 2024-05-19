using ObjectMapper.DTO;
using QueryContracts.Contracts;
using W3TL.Core.Domain.Common.Values;

namespace QueryContracts.Queries;

/// <summary>
///     Represents a query to get posts by user ID.
/// </summary>
/// Usage:
/// var query = new GetPostsByUserIdQuery.Query(userId);
/// var answer = await queryBus.DispatchAsync(query);
/// /
public class GetPostsByUserIdQuery {
    public record Query(UserID UserId) : IQuery<Answer>;

    public record Answer(
        List<ContentDTO> Posts);
}