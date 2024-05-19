using ObjectMapper;
using Persistence.UserPersistence;
using QueryContracts.Contracts;
using QueryContracts.Queries.Users;
using W3TL.Core.Domain.Agregates.User.Repository;

namespace Queries.Queries.User;

///<summary>
/// Handles queries to retrieve all users.
/// Implements the IQueryHandler interface for handling queries.
///</summary>
public class GetAllUsersHandler : IQueryHandler<GetAllUsersQuery.Query, GetAllUsersQuery.Answer> {
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    ///<summary>
    /// Initializes a new instance of the GetAllUsersHandler class.
    ///</summary>
    ///<param name="userRepository">The injected user repository dependency.</param>
    ///<param name="mapper">The injected mapper dependency.</param>
    public GetAllUsersHandler(IUserRepository userRepository, IMapper mapper) {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    ///<summary>
    /// Handles the incoming query to retrieve all users asynchronously.
    ///</summary>
    ///<param name="query">The query containing optional parameters for filtering users.</param>
    ///<returns>An asynchronous task that yields the answer containing the list of users.</returns>
    public async Task<GetAllUsersQuery.Answer> HandleAsync(GetAllUsersQuery.Query query) {
        var users = await _userRepository.GetAllAsync();
        var dtoList = users.Payload.Select(u => _mapper.Map<UserDTO>(u)).ToList();
        return new GetAllUsersQuery.Answer(dtoList);
    }
}