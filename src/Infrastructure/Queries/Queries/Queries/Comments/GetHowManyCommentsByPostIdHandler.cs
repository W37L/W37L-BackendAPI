using QueryContracts.Contracts;
using QueryContracts.Queries.Comments;
using W3TL.Core.Domain.Agregates.Post.Repository;

namespace Queries.Queries.Comments;

public class GetHowManyCommentsByPostIdHandler : IQueryHandler<GetHowManyCommentsByPostIdQuery.Query,
    GetHowManyCommentsByPostIdQuery.Answer> {
    private readonly IContentRepository _contentRepository;

    public GetHowManyCommentsByPostIdHandler(IContentRepository contentRepository) {
        _contentRepository = contentRepository;
    }

    public async Task<GetHowManyCommentsByPostIdQuery.Answer> HandleAsync(GetHowManyCommentsByPostIdQuery.Query query) {
        var comments = await _contentRepository.GetCommentsByPostIdAsync(query.postId);
        if (comments.IsFailure)
            return new GetHowManyCommentsByPostIdQuery.Answer(0);
        return new GetHowManyCommentsByPostIdQuery.Answer(comments.Payload.Count);
    }
}