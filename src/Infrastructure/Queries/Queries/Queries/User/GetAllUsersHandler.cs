using ObjectMapper;
using Persistence.UserPersistence;
using QueryContracts.Contracts;
using QueryContracts.Queries.Users;
using W3TL.Core.Domain.Agregates.User.Repository;

namespace Queries.Queries.User;

public class GetAllUsersHandler : IQueryHandler<GetAllUsersQuery.Query, GetAllUsersQuery.Answer> {
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public GetAllUsersHandler(IUserRepository userRepository, IMapper mapper) {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<GetAllUsersQuery.Answer> HandleAsync(GetAllUsersQuery.Query query) {
        var users = await _userRepository.GetAllAsync();
        var dtoList = users.Payload.Select(u => _mapper.Map<UserDTO>(u)).ToList();
        return new GetAllUsersQuery.Answer(dtoList);
    }
}