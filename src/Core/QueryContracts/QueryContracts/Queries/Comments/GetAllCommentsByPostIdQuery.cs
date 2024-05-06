using ObjectMapper.DTO;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.Comments;

public class GetAllCommentsByPostIdQuery {
    public record Query(PostId PostId) : IQuery<Answer>;

    public record Answer(List<ContentDTO> Comments);
}