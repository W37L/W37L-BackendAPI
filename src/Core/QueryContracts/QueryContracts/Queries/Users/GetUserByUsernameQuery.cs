using Persistence.UserPersistence;
using QueryContracts.Contracts;
using W3TL.Core.Domain.Agregates.User.Values;

namespace QueryContracts.Queries.Users;

/// <summary>
///     Represents a query to get a user by their username.
/// </summary>
public class GetUserByUsernameQuery {
    public record Query(UserNameType username) : IQuery<Answer>;

    public record Answer(UserDTO User);
}