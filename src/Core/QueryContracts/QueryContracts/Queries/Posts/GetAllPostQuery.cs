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
    
    /// <summary>
    ///   Represents a query to get all posts.
    /// </summary>
    public record Query : IQuery<Answer>;

    /// <summary>
    ///  Represents the answer returned by the GetAllPostQuery.Query handler.
    /// </summary>
    /// <param name="Posts"></param>
    public record Answer(List<ContentDTO> Posts);
}