using ObjectMapper;
using ObjectMapper.DTO;
using ObjectMapper.Tools;
using QueryContracts.Contracts;
using QueryContracts.Queries;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.User.Repository;

namespace Queries.Queries.Post;

///<summary>
/// Handles queries to retrieve a post by its ID.
/// Implements the IQueryHandler interface for handling queries.
///</summary>
public class GetPostsQueryHandler : IQueryHandler<GetPostByIdQuery.Query, GetPostByIdQuery.Answer> {
    private readonly IContentRepository _contentRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    ///<summary>
    /// Initializes a new instance of the GetPostsQueryHandler class.
    ///</summary>
    ///<param name="contentRepository">The injected content repository dependency.</param>
    ///<param name="userRepository">The injected user repository dependency.</param>
    ///<param name="mapper">The injected mapper dependency.</param>
    public GetPostsQueryHandler(IContentRepository contentRepository, IUserRepository userRepository, IMapper mapper) {
        _contentRepository = contentRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    ///<summary>
    /// Handles the incoming query to get a post by its ID asynchronously.
    ///</summary>
    ///<param name="byIdQuery">The query containing the ID of the post to retrieve.</param>
    ///<returns>An asynchronous task that yields the answer containing the retrieved post.</returns>
    public async Task<GetPostByIdQuery.Answer> HandleAsync(GetPostByIdQuery.Query byIdQuery) {
        var post = await _contentRepository.GetByIdAsync(byIdQuery.PostId);
        if (post.IsFailure)
            return new GetPostByIdQuery.Answer(null);

        var authorId = await _contentRepository.GetAuthorIdAsync(post.Payload.Id);
        if (authorId.IsFailure)
            throw new Exception(authorId.Error.Message);

        var author = await _userRepository.GetByIdAsync(authorId.Payload);
        if (author.IsFailure)
            throw new Exception(author.Error.Message);

        var concatenated = Concatenate.Append(author.Payload, post.Payload as global::Post);


        return new GetPostByIdQuery.Answer(_mapper.Map<ContentDTO>(concatenated));
    }
}