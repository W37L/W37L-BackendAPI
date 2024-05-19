using QueryContracts.Contracts;

namespace QueryContracts.Queries.Comments;

/// <summary>
/// Represents a query to retrieve the number of comments associated with a specific post ID.
/// </summary>
/// Usage:
/// var query = new GetHowManyCommentsByPostIdQuery.Query(postId);
/// var answer = await queryBus.DispatchAsync(query);
public class GetHowManyCommentsByPostIdQuery {
    
    /// <summary>
    ///  Represents a query for getting the number of comments by post ID.
    /// </summary>
    /// <param name="PostId"></param>
    public record Query(PostId PostId) : IQuery<Answer>;

    /// <summary>
    ///   Represents the answer to the query for getting the number of comments by post ID.
    /// </summary>
    /// <param name="Count"></param>
    public record Answer(int Count);
}