using ObjectMapper;
using Persistence.UserPersistence;
using QueryContracts.Contracts;
using QueryContracts.Queries.Users;
using W3TL.Core.Domain.Agregates.User.Repository;

namespace Queries.Queries.User;

public class GetUserByIdHandler : IQueryHandler<GetUserByIdQuery.Query, GetUserByIdQuery.Answer> {
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public GetUserByIdHandler(IUserRepository userRepository, IMapper mapper) {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<GetUserByIdQuery.Answer> HandleAsync(GetUserByIdQuery.Query query) {
        var user = await _userRepository.GetByIdAsync(query.UserID);
        var dto = _mapper.Map<UserDTO>(user.Payload);
        return new GetUserByIdQuery.Answer(dto);
    }
}