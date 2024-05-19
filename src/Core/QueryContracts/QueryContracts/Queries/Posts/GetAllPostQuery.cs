using ObjectMapper.DTO;
using QueryContracts.Contracts;

namespace QueryContracts.Queries;

/// <summary>
///     Represents a query object used to fetch all posts.
/// </summary>
/// Usage:
/// var query = new GetAllPostQuery.Query();
/// var answer = await queryBus.DispatchAsync(query);
/// /
public class GetAllPostQuery {
    public record Query : IQuery<Answer>;

    /// <summary>
    ///     Represents the result of a GetAllPostQuery.
    /// </summary>
    public record Answer(List<ContentDTO> Posts);
}