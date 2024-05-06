using ObjectMapper;
using Persistence.UserPersistence;
using QueryContracts.Contracts;
using QueryContracts.Queries.Users;
using W3TL.Core.Domain.Agregates.User.Repository;

namespace Queries.Queries.User;

public class GetUserByUsernameHandler : IQueryHandler<GetUserByUsernameQuery.Query, GetUserByUsernameQuery.Answer> {
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public GetUserByUsernameHandler(IUserRepository userRepository, IMapper mapper) {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<GetUserByUsernameQuery.Answer> HandleAsync(GetUserByUsernameQuery.Query query) {
        var user = await _userRepository.GetByUserNameAsync(query.username.Value);

        if (user.IsFailure)
            return new GetUserByUsernameQuery.Answer(null);

        var dto = _mapper.Map<UserDTO>(user.Payload);
        return new GetUserByUsernameQuery.Answer(dto);
    }
}