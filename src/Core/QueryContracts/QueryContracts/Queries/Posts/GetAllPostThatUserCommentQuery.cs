using ObjectMapper.DTO;
using QueryContracts.Contracts;
using W3TL.Core.Domain.Common.Values;

namespace QueryContracts.Queries;

/// <summary>
///     Represents a query to retrieve all posts that a user has commented on.
/// </summary>
/// Usage:
/// var query = new GetAllPostThatUserCommentQuery.Query(userId);
/// var answer = await queryBus.DispatchAsync(query);
/// /
public class GetAllPostThatUserCommentQuery {
    public record Query(UserID UserId) : IQuery<Answer>;

    public record Answer(List<ContentDTO> Posts);
}