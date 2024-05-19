using ObjectMapper;
using ObjectMapper.DTO;
using ObjectMapper.Tools;
using QueryContracts.Contracts;
using QueryContracts.Queries;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.User.Repository;

///<summary>
/// Handler for retrieving all posts.
///</summary>
public class GetAllPostHandler : IQueryHandler<GetAllPostQuery.Query, GetAllPostQuery.Answer> {
    private readonly IMapper _mapper;
    private readonly IContentRepository _postRepository;
    private readonly IUserRepository _userRepository;

    ///<summary>
    /// Constructor for initializing the handler with required dependencies.
    ///</summary>
    ///<param name="postRepository">The post repository for accessing post data.</param>
    ///<param name="mapper">The mapper for mapping entities to DTOs.</param>
    ///<param name="userRepository">The user repository for accessing user data.</param>
    public GetAllPostHandler(IContentRepository postRepository, IMapper mapper, IUserRepository userRepository) {
        _postRepository = postRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    ///<summary>
    /// Handles the query to retrieve all posts asynchronously.
    ///</summary>
    ///<param name="query">The query.</param>
    ///<returns>Returns the answer containing the list of post DTOs.</returns>
    public async Task<GetAllPostQuery.Answer> HandleAsync(GetAllPostQuery.Query query) {
        var posts = await _postRepository.GetAllAsync();
        var list = posts.Payload.Select(p => (Post)p);
        var completeList = new List<Content>();

        foreach (var post in list) {
            // get the post's author
            var authorId = await _postRepository.GetAuthorIdAsync(post.Id);
            var author = _userRepository.GetByIdAsync(authorId.Payload).Result
                .OnSuccess(u => completeList.Add(Concatenate.Append(u, post)));
        }

        var dtoList = completeList.Select(p => _mapper.Map<ContentDTO>(p)).ToList();
        return new GetAllPostQuery.Answer(dtoList);
    }
}