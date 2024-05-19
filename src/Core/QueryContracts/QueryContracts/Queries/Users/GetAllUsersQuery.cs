using Persistence.UserPersistence;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.Users;

/// <summary>
///     Represents a query object for retrieving all users.
/// </summary>
/// Usage:
/// var query = new GetAllUsersQuery.Query();
/// var answer = await queryBus.DispatchAsync(query);
/// /
public class GetAllUsersQuery {
    
    /// <summary>
    ///  Represents a query to get all users.
    /// </summary>
    public record Query : IQuery<Answer>;

    /// <summary>
    ///  Represents the answer returned by the GetAllUsersQuery.Query handler.
    /// </summary>
    /// <param name="Users"></param>
    public record Answer(List<UserDTO> Users);
}