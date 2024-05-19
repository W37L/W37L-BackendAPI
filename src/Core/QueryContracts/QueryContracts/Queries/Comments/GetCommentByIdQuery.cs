using ObjectMapper.DTO;
using QueryContracts.Contracts;
using W3TL.Core.Domain.Agregates.Post.Values;

namespace QueryContracts.Queries.Comments;

/// GetCommentByIdQuery class.
/// Represents a query to get a comment by its ID.
/// Usage:
/// var query = new GetCommentByIdQuery.Query(commentId);
/// var answer = await queryBus.DispatchAsync(query);
public class GetCommentByIdQuery {
    
    /// <summary>
    ///  Represents a query for getting a comment by its ID.
    /// </summary>
    /// <param name="CommentId"></param>
    public record Query(CommentId CommentId) : IQuery<Answer>;

    /// <summary>
    ///  Represents the answer to the query for getting a comment by its ID.
    /// </summary>
    /// <param name="Comment"></param>
    public record Answer(ContentDTO Comment);
}