using ObjectMapper.DTO;
using QueryContracts.Contracts;
using W3TL.Core.Domain.Agregates.Post.Values;

namespace QueryContracts.Queries.Comments;

public class GetCommentByIdQuery {
    public record Query(CommentId CommentId) : IQuery<Answer>;

    public record Answer(ContentDTO Comment);
}