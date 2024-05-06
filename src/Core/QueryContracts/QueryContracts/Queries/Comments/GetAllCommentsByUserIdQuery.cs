using ObjectMapper.DTO;
using QueryContracts.Contracts;
using W3TL.Core.Domain.Common.Values;

namespace QueryContracts.Queries.Comments;

public class GetAllCommentsByUserIdQuery {
    public record Query(UserID UserId) : IQuery<Answer>;

    public record Answer(List<ContentDTO> Comments);
}