using ObjectMapper.DTO;
using QueryContracts.Contracts;

namespace QueryContracts.Queries;

/// <summary>
///     Represents a query to retrieve a post by its ID.
/// </summary>
/// Usage:
/// var query = new GetPostByIdQuery.Query(postId);
/// var answer = await queryBus.DispatchAsync(query);
/// /
public class GetPostByIdQuery {
    public record Query(PostId PostId) : IQuery<Answer>;

    public record Answer(ContentDTO Post);
}