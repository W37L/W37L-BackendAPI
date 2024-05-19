using Persistence.UserPersistence;
using QueryContracts.Contracts;
using W3TL.Core.Domain.Agregates.User.Values;

namespace QueryContracts.Queries.Users;

/// <summary>
///     Represents a query to get a user by their username.
/// </summary>
/// Usage example:
/// var query = new SearchPostsQuery.Query("search term");
/// var answer = await queryBus.DispatchAsync(query); 
public class GetUserByUsernameQuery {
    
    /// <summary>
    ///   Represents a query to get a user by their username.
    /// </summary>
    /// <param name="UserName"></param>
    public record Query(UserNameType UserName) : IQuery<Answer>;

    /// <summary>
    ///  Represents the answer returned by the GetUserByUsernameQuery.Query handler.
    /// </summary>
    /// <param name="User"></param>
    public record Answer(UserDTO User);
}