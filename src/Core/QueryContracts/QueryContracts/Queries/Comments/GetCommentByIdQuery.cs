using ObjectMapper.DTO;
using QueryContracts.Contracts;
using W3TL.Core.Domain.Agregates.Post.Values;

namespace QueryContracts.Queries.Comments;

/// GetCommentByIdQuery class.
/// Represents a query to get a comment by its ID.
/// Usage:
/// var query = new GetCommentByIdQuery.Query(commentId);
/// var answer = await queryBus.DispatchAsync(query);
/// /
public class GetCommentByIdQuery {
    public record Query(CommentId CommentId) : IQuery<Answer>;

    public record Answer(ContentDTO Comment);
}