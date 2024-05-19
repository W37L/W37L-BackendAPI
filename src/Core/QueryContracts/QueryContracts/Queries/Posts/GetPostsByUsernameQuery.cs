using ObjectMapper.DTO;
using QueryContracts.Contracts;

namespace QueryContracts.Queries;

/// <summary>
///     Represents a query to get posts by username.
/// </summary>
/// Usage:
/// var query = new GetPostsByUsernameQuery.Query("someUsername");
/// var answer = await queryBus.DispatchAsync(query);
public class GetPostsByUsernameQuery {
    
    /// <summary>
    ///  Represents a query to get posts by username.
    /// </summary>
    /// <param name="Username"></param>
    public record Query(string Username) : IQuery<Answer>;
    
    /// <summary>
    ///  Represents the answer returned by the GetPostsByUsernameQuery.Query handler.
    /// </summary>
    /// <param name="Posts"></param>
    public record Answer(
        List<ContentDTO> Posts);
}