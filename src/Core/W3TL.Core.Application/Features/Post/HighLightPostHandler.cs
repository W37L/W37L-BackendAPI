using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.Post;

/// <summary>
/// Handles the command for highlighting a post.
/// </summary>
public class HighLightPostHandler : ICommandHandler<HighLighPostCommand> {
    private readonly IContentRepository contentRepository;
    private readonly IInteractionRepository interactionRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IUserRepository userRepository;

    /// <summary>
    ///  Constructor for initializing the handler with required dependencies.
    /// </summary>
    /// <param name="contentRepository"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="userRepository"></param>
    /// <param name="interactionRepository"></param>
    public HighLightPostHandler(
        IContentRepository contentRepository,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IInteractionRepository interactionRepository) {
        this.contentRepository = contentRepository;
        this.unitOfWork = unitOfWork;
        this.userRepository = userRepository;
        this.interactionRepository = interactionRepository;
    }

    /// <summary>
    /// Handles the command to highlight a post asynchronously.
    /// </summary>
    /// <param name="command">The command to highlight a post.</param>
    /// <returns>A result indicating the success or failure of the operation.</returns>
    public async Task<Result> HandleAsync(HighLighPostCommand command) {
        // Search for post by id
        var userResult = await userRepository.GetByIdAsync(command.HighlighterId);
        if (userResult.IsFailure)
            return Error.UserNotFound;

        var result = await contentRepository.GetByFullIdAsync(command.Id, userResult.Payload);

        if (result.IsFailure)
            return Error.PostNotFound;

        if (result.Payload is global::Post post) {
            // Highlight post
            var highlight = post.Highlight(userResult.Payload);

            if (highlight.IsFailure)
                return highlight.Error;

            // Add highlight to repository
            await interactionRepository.HighlightTweetAsync(userResult.Payload.Id.Value, post.Id.Value);
            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        return Error.PostNotFound;
    }
}