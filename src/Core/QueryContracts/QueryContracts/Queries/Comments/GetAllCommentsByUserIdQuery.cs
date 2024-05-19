using ObjectMapper.DTO;
using QueryContracts.Contracts;
using W3TL.Core.Domain.Common.Values;

namespace QueryContracts.Queries.Comments;

/// <summary>
///     Represents a query to get all comments by user ID.
/// </summary>
public class GetAllCommentsByUserIdQuery {
    
    /// <summary>
    ///   Represents a query to get all comments by user ID.
    /// </summary>
    /// <param name="UserId"></param>
    public record Query(UserID UserId) : IQuery<Answer>;

    /// <summary>
    ///   Represents the answer returned by the GetAllCommentsByUserIdQuery.Query handler.
    /// </summary>
    /// <param name="Comments"></param>
    public record Answer(List<ContentDTO> Comments);
}