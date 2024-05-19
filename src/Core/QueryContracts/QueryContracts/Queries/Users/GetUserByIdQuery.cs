using Persistence.UserPersistence;
using QueryContracts.Contracts;
using W3TL.Core.Domain.Common.Values;

namespace QueryContracts.Queries.Users;

/// <summary>
///     Represents a query to retrieve a user by their ID.
/// </summary>
/// Usage:
/// var query = new GetUserByIdQuery.Query(userId);
/// var answer = await queryBus.DispatchAsync(query);
/// /
public class GetUserByIdQuery {
    
    /// <summary>
    ///     Represents a query to retrieve a user by their ID.
    /// </summary>
    /// <param name="UserId"></param>
    public record Query(UserID UserId) : IQuery<Answer>;

    /// <summary>
    ///   Represents the answer returned by the GetUserByIdQuery.Query handler.
    /// </summary>
    /// <param name="User"></param>
    public record Answer(UserDTO User);
}