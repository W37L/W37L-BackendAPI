using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.Post;

/// <summary>
/// Handles the command for liking a post.
/// </summary>
public class LikeContentHandler(
    IContentRepository contentRepository,
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    IInteractionRepository interactionRepository) : ICommandHandler<LikeContentCommand> {
    
    /// <summary>
    /// Handles the command to like a post asynchronously.
    /// </summary>
    /// <param name="command">The command to like a post.</param>
    /// <returns>A result indicating the success or failure of the operation.</returns>
    public async Task<Result> HandleAsync(LikeContentCommand command) {
        // Search for post by id
        var userResult = await userRepository.GetByIdAsync(command.LikerId);
        if (userResult.IsFailure)
            return Error.UserNotFound;

        var result = await contentRepository.GetByFullIdAsync(command.Id, userResult.Payload);

        if (result.IsFailure)
            return Error.PostNotFound;

        if (result.Payload is global::Post post) {
            // Like post
            var like = post.Like(userResult.Payload);

            if (like.IsFailure)
                return like.Error;

            // Add like to repository
            await contentRepository.UpdateAsync(post);
            await interactionRepository.LikeTweetAsync(userResult.Payload.Id.Value, post.Id.Value);
            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        return Error.PostNotFound;
    }
}