using Persistence.UserPersistence;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.Users;

/// <summary>
///     Represents a query object for retrieving all users.
/// </summary>
public class GetAllUsersQuery {
    /// <summary>
    ///     Represents a query for getting all users.
    /// </summary>
    public record Query : IQuery<Answer>;

    /// <summary>
    ///     Represents the answer returned by the GetAllUsersQuery.
    /// </summary>
    public record Answer(List<UserDTO> Users);
}