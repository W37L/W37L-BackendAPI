using ObjectMapper;
using Persistence.UserPersistence.Firebase;
using QueryContracts.Contracts;
using QueryContracts.Queries.Users;
using W3TL.Core.Domain.Agregates.User.Repository;

namespace Queries.Queries.User;

public class GetRelationsHandler : IQueryHandler<GetRelationsQuery.Query, GetRelationsQuery.Answer> {
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public GetRelationsHandler(IUserRepository userRepository, IMapper mapper) {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<GetRelationsQuery.Answer> HandleAsync(GetRelationsQuery.Query query) {
        var user = await _userRepository.GetByIdAsync(query.UserID);
        var interactions = user.Payload.Interactions;

        var interactionsDTO = _mapper.Map<InteractionsDTO>(interactions);

        return new GetRelationsQuery.Answer(interactionsDTO);
    }
}