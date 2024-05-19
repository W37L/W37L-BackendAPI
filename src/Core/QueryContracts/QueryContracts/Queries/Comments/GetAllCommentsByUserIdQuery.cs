using ObjectMapper.DTO;
using QueryContracts.Contracts;
using W3TL.Core.Domain.Common.Values;

namespace QueryContracts.Queries.Comments;

/// <summary>
///     Represents a query to get all comments by user ID.
/// </summary>
public class GetAllCommentsByUserIdQuery {
    /// <summary>
    ///     Represents a query for retrieving all comments by user ID.
    /// </summary>
    public record Query(UserID UserId) : IQuery<Answer>;

    /// <summary>
    ///     Represents the answer to the query for getting all comments by user ID.
    /// </summary>
    public record Answer(List<ContentDTO> Comments);
}