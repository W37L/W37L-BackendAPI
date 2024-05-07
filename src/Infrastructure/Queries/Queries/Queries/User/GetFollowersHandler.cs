using ObjectMapper;
using Persistence.UserPersistence.Firebase;
using QueryContracts.Contracts;
using QueryContracts.Queries.Users;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.Values;

namespace Queries.Queries.User;

public class GetFollowersHandler : IQueryHandler<GetFollowersQuery.Query, GetFollowersQuery.Answer> {
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;


    public GetFollowersHandler(IUserRepository userRepository, IMapper mapper) {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<GetFollowersQuery.Answer> HandleAsync(GetFollowersQuery.Query query) {
        var user = await _userRepository.GetByIdAsync(query.UserID);
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