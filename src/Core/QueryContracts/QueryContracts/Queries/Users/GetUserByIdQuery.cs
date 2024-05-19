using Persistence.UserPersistence;
using QueryContracts.Contracts;
using W3TL.Core.Domain.Common.Values;

namespace QueryContracts.Queries.Users;

/// <summary>
///     Represents a query to retrieve a user by their ID.
/// </summary>
public class GetUserByIdQuery {
    public record Query(UserID UserID) : IQuery<Answer>;

    public record Answer(UserDTO User);
}