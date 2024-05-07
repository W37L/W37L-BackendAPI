using ObjectMapper;
using Persistence.UserPersistence.Firebase;
using QueryContracts.Contracts;
using QueryContracts.Queries.Users;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.Values;

namespace Queries.Queries.User;

public class GetFollowingHandler : IQueryHandler<GetFollowingQuery.Query, GetFollowingQuery.Answer> {
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public GetFollowingHandler(IUserRepository userRepository, IMapper mapper) {
        _userRepository = userRepository;
        _mapper = mapper;
    }

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