using QueryContracts.Contracts;

namespace QueryContracts.Queries.Comments;

/// <summary>
///     Represents a query that retrieves the number of comments by post ID.
/// </summary>
public class GetHowManyCommentsByPostIdQuery {
    public record Query(PostId postId) : IQuery<Answer>;

    public record Answer(int Count);
}