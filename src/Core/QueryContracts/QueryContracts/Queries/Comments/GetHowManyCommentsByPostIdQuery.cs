using QueryContracts.Contracts;

namespace QueryContracts.Queries.Comments;

/// <summary>
/// Represents a query to retrieve the number of comments associated with a specific post ID.
/// </summary>
/// Usage:
/// var query = new GetHowManyCommentsByPostIdQuery.Query(postId);
/// var answer = await queryBus.DispatchAsync(query);
/// /
public class GetHowManyCommentsByPostIdQuery {
    public record Query(PostId postId) : IQuery<Answer>;

    public record Answer(int Count);
}