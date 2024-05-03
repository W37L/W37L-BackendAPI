using ObjectMapper;
using ObjectMapper.DTO;
using QueryContracts.Contracts;
using QueryContracts.Queries;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.User.Repository;

namespace Queries.Queries.Post;

public class GetPostsByUserIdHandler : IQueryHandler<GetPostsByUserIdQuery.Query, GetPostsByUserIdQuery.Answer> {
    private readonly IContentRepository _contentRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public GetPostsByUserIdHandler(IContentRepository contentRepository, IUserRepository userRepository,
        IMapper mapper) {
        _contentRepository = contentRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<GetPostsByUserIdQuery.Answer> HandleAsync(GetPostsByUserIdQuery.Query query) {
        var user = await _userRepository.GetByIdAsync(query.UserId);
        var posts = await _contentRepository.GetPostsByUserIdAsync(query.UserId);

        var postDTOs = posts.Payload.Select(p => _mapper.Map<ContentDTO>(p)).ToList();

        return new GetPostsByUserIdQuery.Answer(postDTOs);
    }
}