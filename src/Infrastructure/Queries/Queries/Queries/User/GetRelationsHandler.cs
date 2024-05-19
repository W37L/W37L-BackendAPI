using ObjectMapper;
using Persistence.UserPersistence.Firebase;
using QueryContracts.Contracts;
using QueryContracts.Queries.Users;
using W3TL.Core.Domain.Agregates.User.Repository;

namespace Queries.Queries.User;

///<summary>
/// Handles queries to retrieve all relations of a user (followers and following).
/// Implements the IQueryHandler interface for handling queries.
///</summary>
public class GetRelationsHandler : IQueryHandler<GetRelationsQuery.Query, GetRelationsQuery.Answer> {
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    ///<summary>
    /// Initializes a new instance of the GetRelationsHandler class.
    ///</summary>
    ///<param name="userRepository">The injected user repository dependency.</param>
    ///<param name="mapper">The injected mapper dependency.</param>
    public GetRelationsHandler(IUserRepository userRepository, IMapper mapper) {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    ///<summary>
    /// Handles the incoming query to retrieve all relations of a user asynchronously.
    ///</summary>
    ///<param name="query">The query containing the user ID for which relations are to be retrieved.</param>
    ///<returns>An asynchronous task that yields the answer containing the user's interactions DTO.</returns>
    public async Task<GetRelationsQuery.Answer> HandleAsync(GetRelationsQuery.Query query) {
        var user = await _userRepository.GetByIdAsync(query.UserID);
        var interactions = user.Payload.Interactions;

        var interactionsDTO = _mapper.Map<InteractionsDTO>(interactions);

        return new GetRelationsQuery.Answer(interactionsDTO);
    }
}