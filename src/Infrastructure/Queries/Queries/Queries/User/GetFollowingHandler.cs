using ObjectMapper;
using Persistence.UserPersistence.Firebase;
using QueryContracts.Contracts;
using QueryContracts.Queries.Users;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.Values;

namespace Queries.Queries.User;

///<summary>
/// Handles queries to retrieve users whom a particular user is following.
/// Implements the IQueryHandler interface for handling queries.
///</summary>
public class GetFollowingHandler : IQueryHandler<GetFollowingQuery.Query, GetFollowingQuery.Answer> {
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    ///<summary>
    /// Initializes a new instance of the GetFollowingHandler class.
    ///</summary>
    ///<param name="userRepository">The injected user repository dependency.</param>
    ///<param name="mapper">The injected mapper dependency.</param>
    public GetFollowingHandler(IUserRepository userRepository, IMapper mapper) {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    ///<summary>
    /// Handles the incoming query to retrieve users whom a particular user is following asynchronously.
    ///</summary>
    ///<param name="query">The query containing the user ID for which following users are to be retrieved.</param>
    ///<returns>An asynchronous task that yields the answer containing the list of following user IDs.</returns>
    public async Task<GetFollowingQuery.Answer> HandleAsync(GetFollowingQuery.Query query) {
        var user = await _userRepository.GetByIdAsync(query.UserID);
        var interactions = user.Payload.Interactions;

        var interactionsDTO = _mapper.Map<InteractionsDTO>(interactions);

        var following = new List<UserID>();

        foreach (var follow in interactionsDTO.following) {
            var userIdResult = UserID.Create(follow);
            if (userIdResult.IsFailure) throw new Exception(userIdResult.Error.Message);

            following.Add(userIdResult.Payload);
        }

        return new GetFollowingQuery.Answer(following);
    }
}