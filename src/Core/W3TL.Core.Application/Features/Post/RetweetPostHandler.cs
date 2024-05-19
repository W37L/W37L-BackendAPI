using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.Post;

/// <summary>
/// Handles the command for retweeting a post.
/// </summary>
public class RetweetPostHandler : ICommandHandler<RetweetPostCommand> {
    private readonly IContentRepository contentRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IUserRepository userRepository;
    private IInteractionRepository interactionRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="RetweetPostHandler"/> class.
    /// </summary>
    /// <param name="contentRepository">The repository for accessing content.</param>
    /// <param name="unitOfWork">The unit of work for managing transactions.</param>
    /// <param name="userRepository">The repository for accessing user data.</param>
    /// <param name="interactionRepository">The repository for handling interactions.</param>
    public RetweetPostHandler(
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
    /// Handles the command to retweet a post asynchronously.
    /// </summary>
    /// <param name="command">The command to retweet a post.</param>
    /// <returns>A result indicating the success or failure of the operation.</returns>
    public async Task<Result> HandleAsync(RetweetPostCommand command) {
        // Search for post by id
        var userResult = await userRepository.GetByIdAsync(command.RetweeterId);
        if (userResult.IsFailure)
            return Error.UserNotFound;

        var result = await contentRepository.GetByFullIdAsync(command.Id, userResult.Payload);

        if (result.IsFailure)
            return Error.PostNotFound;

        if (result.Payload is global::Post post) {
            // Retweet post
            var retweet = post.Retweet(userResult.Payload);

            if (retweet.IsFailure)
                return retweet.Error;

            // Add retweet to repository
            await interactionRepository.RetweetAsync(userResult.Payload.Id.Value, post.Id.Value);
            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        return Error.PostNotFound;
    }
}