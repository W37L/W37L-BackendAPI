using ObjectMapper;
using ObjectMapper.DTO;
using ObjectMapper.Tools;
using QueryContracts.Contracts;
using QueryContracts.Queries;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.User.Repository;

namespace Queries.Queries.Post;

public class
    GetAllPostThatUserCommentHandler : IQueryHandler<GetAllPostThatUserCommentQuery.Query,
    GetAllPostThatUserCommentQuery.Answer> {
    private readonly IContentRepository _contentRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;


    public GetAllPostThatUserCommentHandler(IContentRepository contentRepository, IMapper mapper,
        IUserRepository userRepository) {
        _contentRepository = contentRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<GetAllPostThatUserCommentQuery.Answer> HandleAsync(GetAllPostThatUserCommentQuery.Query query) {
        var posts = await _contentRepository.GetAllPostThatUserCommentAsync(query.UserId);
        var list = posts.Payload.Select(p => (global::Post)p);
        var completeList = new List<Content>();

        foreach (var post in list) {
            var authorId = await _contentRepository.GetAuthorIdAsync(post.Id);
            var author = _userRepository.GetByIdAsync(authorId.Payload).Result
                .OnSuccess(u => completeList.Add(Concatenate.Append(u, post)));
        }

        var dtoList = completeList.Select(p => _mapper.Map<ContentDTO>(p)).ToList();
        return new GetAllPostThatUserCommentQuery.Answer(dtoList);
    }
}