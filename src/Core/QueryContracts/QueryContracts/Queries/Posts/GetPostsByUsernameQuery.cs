using ObjectMapper.DTO;
using QueryContracts.Contracts;

namespace QueryContracts.Queries;

/// <summary>
///     Represents a query to get posts by username.
/// </summary>
public class GetPostsByUsernameQuery {
    public record Query(string Username) : IQuery<Answer>;

    public record Answer(
        List<ContentDTO> Posts);
}