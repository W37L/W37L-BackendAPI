using ObjectMapper;
using ObjectMapper.DTO;
using ObjectMapper.Tools;
using QueryContracts.Contracts;
using QueryContracts.Queries.Comments;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.User.Repository;

namespace Queries.Queries.Comments;

public class GetCommentByIdHandler : IQueryHandler<GetCommentByIdQuery.Query, GetCommentByIdQuery.Answer> {
    private readonly IMapper _mapper;

    private readonly IContentRepository _postRepository;
    private readonly IUserRepository _userRepository;


    public GetCommentByIdHandler(IContentRepository postRepository, IMapper mapper, IUserRepository userRepository) {
        _postRepository = postRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

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