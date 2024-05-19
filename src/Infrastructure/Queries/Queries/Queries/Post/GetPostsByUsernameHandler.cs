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
/// Represents a handler for retrieving posts by username.
/// Implements the IQueryHandler interface for handling queries.
///</summary>
public class GetPostsByUsernameHandler : IQueryHandler<GetPostsByUsernameQuery.Query, GetPostsByUsernameQuery.Answer> {
    private readonly IContentRepository _contentRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    ///<summary>
    /// Initializes a new instance of the GetPostsByUsernameHandler class.
    ///</summary>
    ///<param name="contentRepository">The injected content repository dependency.</param>
    ///<param name="userRepository">The injected user repository dependency.</param>
    ///<param name="mapper">The injected mapper dependency.</param>
    public GetPostsByUsernameHandler(IContentRepository contentRepository, IUserRepository userRepository,
        IMapper mapper) {
        _contentRepository = contentRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    ///<summary>
    /// Handles the incoming query to get posts by username asynchronously.
    ///</summary>
    ///<param name="query">The query containing the username to retrieve posts for.</param>
    ///<returns>An asynchronous task that yields the answer containing the posts.</returns>
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