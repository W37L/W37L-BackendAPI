using QueryContracts.Contracts;
using QueryContracts.Queries.Comments;
using W3TL.Core.Domain.Agregates.Post.Repository;

namespace Queries.Queries.Comments;

///<summary>
///Handler for querying the number of comments by post ID.
///</summary>
public class GetHowManyCommentsByPostIdHandler : IQueryHandler<GetHowManyCommentsByPostIdQuery.Query, GetHowManyCommentsByPostIdQuery.Answer> {
    private readonly IContentRepository _contentRepository;

    /// <summary>
    /// Constructor for initializing the handler with the content repository.
    /// </summary>
    /// <param name="contentRepository">The content repository.</param>
    public GetHowManyCommentsByPostIdHandler(IContentRepository contentRepository) {
        _contentRepository = contentRepository;
    }

    /// <summary>
    /// Handles the query to get the number of comments by post ID.
    /// </summary>
    /// <param name="query">The query containing the post ID.</param>
    /// <returns>Returns the number of comments as the query answer.</returns>
    public async Task<GetHowManyCommentsByPostIdQuery.Answer> HandleAsync(GetHowManyCommentsByPostIdQuery.Query query) {
        var comments = await _contentRepository.GetCommentsByPostIdAsync(query.PostId);
        if (comments.IsFailure)
            return new GetHowManyCommentsByPostIdQuery.Answer(0);
        return new GetHowManyCommentsByPostIdQuery.Answer(comments.Payload.Count);
    }
}