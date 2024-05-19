using ObjectMapper;
using ObjectMapper.DTO;
using ObjectMapper.Tools;
using QueryContracts.Contracts;
using QueryContracts.Queries.Comments;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.User.Repository;

namespace Queries.Queries.Comments;

///<summary>
/// Handler for retrieving all comments by a specific post ID.
///</summary>
public class GetAllCommentsByPostIdHandler : IQueryHandler<GetAllCommentsByPostIdQuery.Query, GetAllCommentsByPostIdQuery.Answer> {
    private readonly IContentRepository _contentRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    ///<summary>
    /// Constructor for initializing the handler with required dependencies.
    ///</summary>
    ///<param name="contentRepository">The content repository for accessing comment data.</param>
    ///<param name="mapper">The mapper for mapping entities to DTOs.</param>
    ///<param name="userRepository">The user repository for accessing user data.</param>
    public GetAllCommentsByPostIdHandler(IContentRepository contentRepository, IMapper mapper,
        IUserRepository userRepository) {
        _contentRepository = contentRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<GetAllCommentsByPostIdQuery.Answer> HandleAsync(GetAllCommentsByPostIdQuery.Query query) {
        var comments = await _contentRepository.GetCommentsByPostIdAsync(query.PostId);
        var list = comments.Payload.Select(c => (Comment)c);
        if (comments.IsFailure)
            return new GetAllCommentsByPostIdQuery.Answer(null);
        var completeList = new List<Content>();

        foreach (var comment in list) {
            var authorId = await _contentRepository.GetAuthorIdAsync(comment.Id);
            var author = _userRepository.GetByIdAsync(authorId.Payload).Result
                .OnSuccess(u => completeList.Add(Concatenate.Append(u, comment)));
        }

        var dtoList = completeList.Select(c => _mapper.Map<ContentDTO>(c)).ToList();
        return new GetAllCommentsByPostIdQuery.Answer(dtoList);
    }
}