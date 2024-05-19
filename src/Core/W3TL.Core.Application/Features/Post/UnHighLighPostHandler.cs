using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.Post;

/// <summary>
/// Handles the command for removing a highlight from a post.
/// </summary>
public class UnHighLighPostHandler : ICommandHandler<UnHighlighPostCommand> {
    private readonly IContentRepository contentRepository;
    private readonly IInteractionRepository interactionRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IUserRepository userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnHighLighPostHandler"/> class.
    /// </summary>
    /// <param name="contentRepository">The repository for accessing content.</param>
    /// <param name="unitOfWork">The unit of work for managing transactions.</param>
    /// <param name="userRepository">The repository for accessing user data.</param>
    /// <param name="interactionRepository">The repository for handling interactions.</param>
    public UnHighLighPostHandler(
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
    /// Handles the command to remove a highlight from a post asynchronously.
    /// </summary>
    /// <param name="command">The command to remove a highlight from a post.</param>
    /// <returns>A result indicating the success or failure of the operation.</returns>
    public async Task<Result> HandleAsync(UnHighlighPostCommand command) {
        // Search for post by id
        var userResult = await userRepository.GetByIdAsync(command.HighlighterId);
        if (userResult.IsFailure)
            return Error.UserNotFound;

        var result = await contentRepository.GetByFullIdAsync(command.Id, userResult.Payload);

        if (result.IsFailure)
            return Error.PostNotFound;

        if (result.Payload is global::Post post) {
            // Unhighlight post
            var unhighlight = post.Unhighlight(userResult.Payload);

            if (unhighlight.IsFailure)
                return unhighlight.Error;

            // Add unhighlight to repository
            await interactionRepository.UnhighlightTweetAsync(userResult.Payload.Id.Value, post.Id.Value);
            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        return Error.PostNotFound;
    }
}