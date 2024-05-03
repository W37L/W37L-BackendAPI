using ObjectMapper.DTO;
using QueryContracts.Contracts;

namespace QueryContracts.Queries;

public class GetPostByIdQuery {
    public record Query(PostId PostId) : IQuery<Answer>;

    public record Answer(ContentDTO Post);
}