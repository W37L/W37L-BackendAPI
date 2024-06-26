using ObjectMapper;
using ObjectMapper.DTO;
using ObjectMapper.Tools;
using QueryContracts.Contracts;
using QueryContracts.Queries.Comments;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Agregates.User.Repository;

namespace Queries.Queries.Comments;

///<summary>
/// Handler for retrieving all comments by a specific user ID.
///</summary>
public class GetAllCommentByUserIdHandler : IQueryHandler<GetAllCommentsByUserIdQuery.Query, GetAllCommentsByUserIdQuery.Answer> {
    private readonly IContentRepository _contentRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    ///<summary>
    /// Constructor for initializing the handler with required dependencies.
    ///</summary>
    ///<param name="contentRepository">The content repository for accessing comment data.</param>
    ///<param name="mapper">The mapper for mapping entities to DTOs.</param>
    ///<param name="userRepository">The user repository for accessing user data.</param>
    public GetAllCommentByUserIdHandler(IContentRepository contentRepository, IMapper mapper,
        IUserRepository userRepository) {
        _contentRepository = contentRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    ///<summary>
    /// Handles the query to retrieve all comments by a specific user ID asynchronously.
    ///</summary>
    ///<param name="query">The query containing the user ID.</param>
    ///<returns>Returns the answer containing the list of comments.</returns>
    public async Task<GetAllCommentsByUserIdQuery.Answer> HandleAsync(GetAllCommentsByUserIdQuery.Query query) {
        var user = await _userRepository.GetByIdAsync(query.UserId);
        if (user.IsFailure)
            throw new Exception(user.Error.Message);

        var comments = await _contentRepository.GetCommentsByUserIdAsync(query.UserId);
        var list = comments.Payload.Select(c => (Comment)c);
        if (comments.IsFailure)
            return new GetAllCommentsByUserIdQuery.Answer(null);
        var completeList = new List<Comment>();

        foreach (var comment in list) {
            var parentId = await _contentRepository.GetParentPostIdAsync(comment.Id as CommentId);
            var parentPost = await _contentRepository.GetByFullIdAsync(parentId.Payload, user.Payload);

            if (parentPost.IsFailure)
                throw new Exception(parentPost.Error.Message);

            var result = Concatenate.Append(parentPost.Payload as global::Post, comment);
            result = (Comment)Concatenate.Append(user.Payload, result);

            completeList.Add(result);
        }

        var dtoList = completeList.Select(c => _mapper.Map<ContentDTO>(c)).ToList();
        return new GetAllCommentsByUserIdQuery.Answer(dtoList);
    }
}