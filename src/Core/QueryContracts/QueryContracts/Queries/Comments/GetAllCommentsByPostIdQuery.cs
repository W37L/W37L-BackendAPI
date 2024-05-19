using ObjectMapper.DTO;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.Comments;

/// <summary>
///     Represents a query to get all comments by post ID.
/// </summary>
public class GetAllCommentsByPostIdQuery {
    /// <summary>
    ///     Represents a query to get all comments by post ID.
    /// </summary>
    public record Query(PostId PostId) : IQuery<Answer>;

    /// <summary>
    ///     Represents the answer returned by the GetAllCommentsByPostIdQuery.Query handler.
    /// </summary>
    public record Answer(List<ContentDTO> Comments);
}