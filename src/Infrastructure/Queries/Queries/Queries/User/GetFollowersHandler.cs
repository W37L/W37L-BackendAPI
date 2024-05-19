using ObjectMapper;
using Persistence.UserPersistence.Firebase;
using QueryContracts.Contracts;
using QueryContracts.Queries.Users;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.Values;

namespace Queries.Queries.User;

///<summary>
/// Handles queries to retrieve followers of a user.
/// Implements the IQueryHandler interface for handling queries.
///</summary>
public class GetFollowersHandler : IQueryHandler<GetFollowersQuery.Query, GetFollowersQuery.Answer> {
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    ///<summary>
    /// Initializes a new instance of the GetFollowersHandler class.
    ///</summary>
    ///<param name="userRepository">The injected user repository dependency.</param>
    ///<param name="mapper">The injected mapper dependency.</param>
    public GetFollowersHandler(IUserRepository userRepository, IMapper mapper) {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    ///<summary>
    /// Handles the incoming query to retrieve followers of a user asynchronously.
    ///</summary>
    ///<param name="query">The query containing the user ID for which followers are to be retrieved.</param>
    ///<returns>An asynchronous task that yields the answer containing the list of follower user IDs.</returns>
    public async Task<GetFollowersQuery.Answer> HandleAsync(GetFollowersQuery.Query query) {
        var user = await _userRepository.GetByIdAsync(query.UserId);
        var interactions = user.Payload.Interactions;

        var interactionsDTO = _mapper.Map<InteractionsDTO>(interactions);

        var followers = new List<UserID>();

        foreach (var follower in interactionsDTO.followers) {
            var userIdResult = UserID.Create(follower);
            if (userIdResult.IsFailure) throw new Exception(userIdResult.Error.Message);

            followers.Add(userIdResult.Payload);
        }

        return new GetFollowersQuery.Answer(followers);
    }
}