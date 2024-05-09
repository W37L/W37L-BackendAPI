using ObjectMapper;
using ObjectMapper.DTO;
using ObjectMapper.Tools;
using QueryContracts.Contracts;
using QueryContracts.Queries;
using W3TL.Core.Domain.Agregates.Post;
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
        var completeList = new List<Content>();

        foreach (var post in posts.Payload) {
            var authorId = await _contentRepository.GetAuthorIdAsync(post.Id);
            var author = await _userRepository.GetByIdAsync(authorId.Payload);
            completeList.Add(Concatenate.Append(author.Payload, post));
        }

        var dtoList = completeList.Select(p => _mapper.Map<ContentDTO>(p)).ToList();
        return new GetPostsByUserIdQuery.Answer(dtoList);
    }
}