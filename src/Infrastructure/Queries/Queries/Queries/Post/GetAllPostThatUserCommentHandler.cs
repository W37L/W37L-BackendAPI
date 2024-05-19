using ObjectMapper;
using ObjectMapper.DTO;
using ObjectMapper.Tools;
using QueryContracts.Contracts;
using QueryContracts.Queries;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.User.Repository;

namespace Queries.Queries.Post;

///<summary>
/// Handler for retrieving all posts where a user has commented.
///</summary>
public class
    GetAllPostThatUserCommentHandler : IQueryHandler<GetAllPostThatUserCommentQuery.Query,
    GetAllPostThatUserCommentQuery.Answer> {
    private readonly IContentRepository _contentRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    ///<summary>
    /// Constructor for initializing the handler with required dependencies.
    ///</summary>
    ///<param name="contentRepository">The content repository for accessing post data.</param>
    ///<param name="mapper">The mapper for mapping entities to DTOs.</param>
    ///<param name="userRepository">The user repository for accessing user data.</param>
    public GetAllPostThatUserCommentHandler(IContentRepository contentRepository, IMapper mapper,
        IUserRepository userRepository) {
        _contentRepository = contentRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    ///<summary>
    /// Handles the query to retrieve all posts where a user has commented asynchronously.
    ///</summary>
    ///<param name="query">The query containing the user ID.</param>
    ///<returns>Returns the answer containing the list of post DTOs.</returns>
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