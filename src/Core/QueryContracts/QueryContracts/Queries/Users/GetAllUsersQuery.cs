using Persistence.UserPersistence;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.Users;

public class GetAllUsersQuery {
    public record Query : IQuery<Answer>;

    public record Answer(List<UserDTO> Users);
}