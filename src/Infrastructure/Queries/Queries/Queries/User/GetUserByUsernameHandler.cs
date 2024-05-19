using ObjectMapper;
using Persistence.UserPersistence;
using QueryContracts.Contracts;
using QueryContracts.Queries.Users;
using W3TL.Core.Domain.Agregates.User.Repository;

namespace Queries.Queries.User;

///<summary>
/// Handles queries to retrieve a user by username.
/// Implements the IQueryHandler interface for handling queries.
///</summary>
public class GetUserByUsernameHandler : IQueryHandler<GetUserByUsernameQuery.Query, GetUserByUsernameQuery.Answer> {
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    ///<summary>
    /// Initializes a new instance of the GetUserByUsernameHandler class.
    ///</summary>
    ///<param name="userRepository">The injected user repository dependency.</param>
    ///<param name="mapper">The injected mapper dependency.</param>
    public GetUserByUsernameHandler(IUserRepository userRepository, IMapper mapper) {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    ///<summary>
    /// Handles the incoming query to retrieve a user by username asynchronously.
    ///</summary>
    ///<param name="query">The query containing the username to retrieve.</param>
    ///<returns>An asynchronous task that yields the answer containing the retrieved user DTO.</returns>
    public async Task<GetUserByUsernameQuery.Answer> HandleAsync(GetUserByUsernameQuery.Query query) {
        var user = await _userRepository.GetByUserNameAsync(query.UserName.Value);

        if (user.IsFailure)
            return new GetUserByUsernameQuery.Answer(null);

        var dto = _mapper.Map<UserDTO>(user.Payload);
        return new GetUserByUsernameQuery.Answer(dto);
    }
}