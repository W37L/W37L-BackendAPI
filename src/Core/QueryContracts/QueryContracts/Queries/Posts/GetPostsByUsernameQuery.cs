using ObjectMapper.DTO;
using QueryContracts.Contracts;

namespace QueryContracts.Queries;

/// <summary>
///     Represents a query to get posts by username.
/// </summary>
/// Usage:
/// var query = new GetPostsByUsernameQuery.Query("someUsername");
/// var answer = await queryBus.DispatchAsync(query);
/// /
public class GetPostsByUsernameQuery {
    public record Query(string Username) : IQuery<Answer>;

    public record Answer(
        List<ContentDTO> Posts);
}