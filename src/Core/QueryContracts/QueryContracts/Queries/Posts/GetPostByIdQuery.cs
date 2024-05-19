using ObjectMapper.DTO;
using QueryContracts.Contracts;

namespace QueryContracts.Queries;

/// <summary>
///     Represents a query to retrieve a post by its ID.
/// </summary>
/// Usage:
/// var query = new GetPostByIdQuery.Query(postId);
/// var answer = await queryBus.DispatchAsync(query);
public class GetPostByIdQuery {
    
    /// <summary>
    ///  Represents a query to get a post by its ID.
    /// </summary>
    /// <param name="PostId"></param>
    public record Query(PostId PostId) : IQuery<Answer>;

    /// <summary>
    ///     Represents the answer to the query for getting a post by its ID.
    /// </summary>
    /// <param name="Post"></param>
    public record Answer(ContentDTO Post);
}