using ObjectMapper;
using ObjectMapper.DTO;
using ObjectMapper.Tools;
using QueryContracts.Contracts;
using QueryContracts.Queries;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.User.Repository;

namespace Queries.Queries.Post;

public class GetPostsByUsernameHandler : IQueryHandler<GetPostsByUsernameQuery.Query, GetPostsByUsernameQuery.Answer> {
    private readonly IContentRepository _contentRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public GetPostsByUsernameHandler(IContentRepository contentRepository, IUserRepository userRepository,
        IMapper mapper) {
        _contentRepository = contentRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<GetPostsByUsernameQuery.Answer> HandleAsync(GetPostsByUsernameQuery.Query query) {
        var user = await _userRepository.GetIdByUsernameAsync(query.Username);
        var posts = await _contentRepository.GetPostsByUserIdAsync(user.Payload.Id);
        var completeList = new List<Content>();

        foreach (var post in posts.Payload) {
            var authorId = await _contentRepository.GetAuthorIdAsync(post.Id);
            var author = await _userRepository.GetByIdAsync(authorId.Payload);
            completeList.Add(Concatenate.Append(author.Payload, post));
        }

        var dtoList = completeList.Select(p => _mapper.Map<ContentDTO>(p)).ToList();
        return new GetPostsByUsernameQuery.Answer(dtoList);
    }
}