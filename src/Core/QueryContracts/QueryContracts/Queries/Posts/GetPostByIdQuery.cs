using ObjectMapper.DTO;
using QueryContracts.Contracts;

namespace QueryContracts.Queries;

/// <summary>
///     Represents a query to retrieve a post by its ID.
/// </summary>
public class GetPostByIdQuery {
    public record Query(PostId PostId) : IQuery<Answer>;

    public record Answer(ContentDTO Post);
}