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
public class GetPostsByUserIdHandler : IQueryHandler<GetPostsByUserIdQuery.Query, GetPostsByUserIdQuery.Answer> {
    private readonly IContentRepository _contentRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    ///<summary>
    /// Constructor for initializing the handler with required dependencies.
    ///</summary>
    ///<param name="contentRepository">The content repository for accessing post data.</param>
    ///<param name="mapper">The mapper for mapping entities to DTOs.</param>
    ///<param name="userRepository">The user repository for accessing user data.</param>
    public GetPostsByUserIdHandler(IContentRepository contentRepository, IUserRepository userRepository,
        IMapper mapper) {
        _contentRepository = contentRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    ///<summary>
    /// Handles the query to retrieve all posts where a user has commented asynchronously.
    ///</summary>
    ///<param name="query">The query containing the user ID.</param>
    ///<returns>Returns the answer containing the list of post DTOs.</returns>
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