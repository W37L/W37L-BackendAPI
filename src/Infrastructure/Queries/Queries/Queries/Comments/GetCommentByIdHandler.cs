using ObjectMapper;
using ObjectMapper.DTO;
using ObjectMapper.Tools;
using QueryContracts.Contracts;
using QueryContracts.Queries.Comments;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.User.Repository;

namespace Queries.Queries.Comments;

///<summary>
/// Handler for retrieving a comment by its ID.
///</summary>
public class GetCommentByIdHandler : IQueryHandler<GetCommentByIdQuery.Query, GetCommentByIdQuery.Answer> {
    private readonly IMapper _mapper;
    private readonly IContentRepository _postRepository;
    private readonly IUserRepository _userRepository;

    ///<summary>
    /// Constructor for initializing the handler with required dependencies.
    ///</summary>
    ///<param name="postRepository">The post repository for accessing post data.</param>
    ///<param name="mapper">The mapper for mapping entities to DTOs.</param>
    ///<param name="userRepository">The user repository for accessing user data.</param>
    public GetCommentByIdHandler(IContentRepository postRepository, IMapper mapper, IUserRepository userRepository) {
        _postRepository = postRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    ///<summary>
    /// Handles the query to retrieve a comment by its ID asynchronously.
    ///</summary>
    ///<param name="query">The query containing the comment ID.</param>
    ///<returns>Returns the answer containing the comment DTO.</returns>
    public async Task<GetCommentByIdQuery.Answer> HandleAsync(GetCommentByIdQuery.Query query) {
        // get the post's author
        var authorId = await _postRepository.GetAuthorIdAsync(query.CommentId);

        var author = await _userRepository.GetByIdAsync(authorId.Payload);

        var post = await _postRepository.GetCommentByIdAsync(query.CommentId);

        //get the comment parent post
        var parentPostId = await _postRepository.GetParentPostIdAsync(query.CommentId);

        var parentPost = await _postRepository.GetByIdAsync(parentPostId.Payload);

        post = Concatenate.Append(author.Payload, post.Payload);

        post = Concatenate.Append(parentPost.Payload as global::Post, post.Payload as global::Comment);

        var dto = _mapper.Map<ContentDTO>(post.Payload);
        return new GetCommentByIdQuery.Answer(dto);
    }
}