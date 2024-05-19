using ObjectMapper;
using Persistence.UserPersistence;
using QueryContracts.Contracts;
using QueryContracts.Queries.Users;
using W3TL.Core.Domain.Agregates.User.Repository;

namespace Queries.Queries.User;

///<summary>
/// Handles queries to retrieve a user by ID.
/// Implements the IQueryHandler interface for handling queries.
///</summary>
public class GetUserByIdHandler : IQueryHandler<GetUserByIdQuery.Query, GetUserByIdQuery.Answer> {
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    ///<summary>
    /// Initializes a new instance of the GetUserByIdHandler class.
    ///</summary>
    ///<param name="userRepository">The injected user repository dependency.</param>
    ///<param name="mapper">The injected mapper dependency.</param>
    public GetUserByIdHandler(IUserRepository userRepository, IMapper mapper) {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    ///<summary>
    /// Handles the incoming query to retrieve a user by ID asynchronously.
    ///</summary>
    ///<param name="query">The query containing the user ID to retrieve.</param>
    ///<returns>An asynchronous task that yields the answer containing the retrieved user DTO.</returns>
    public async Task<GetUserByIdQuery.Answer> HandleAsync(GetUserByIdQuery.Query query) {
        var user = await _userRepository.GetByIdAsync(query.UserId);
        var dto = _mapper.Map<UserDTO>(user.Payload);
        return new GetUserByIdQuery.Answer(dto);
    }
}