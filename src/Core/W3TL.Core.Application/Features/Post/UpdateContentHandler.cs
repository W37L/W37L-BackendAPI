using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.Post;

/// <summary>
/// Handles the command for updating the content of a post.
/// </summary>
public class UpdateContentHandler(
    IContentRepository contentRepository, 
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateContentCommand> {
    
    /// <summary>
    /// Handles the command to update the content of a post asynchronously.
    /// </summary>
    /// <param name="command">The command to update the content of a post.</param>
    /// <returns>A result indicating the success or failure of the operation.</returns>
    public async Task<Result> HandleAsync(UpdateContentCommand command) {
        // Search for post by id
        var result = await contentRepository.GetByIdAsync(command.Id);
        if (result.IsFailure)
            return Error.ContentNotFound;

        // Update post
        var post = result.Payload;
        var action = post.EditContent(command.ContentTweet, command.Signature);

        if (action.IsFailure)
            return action.Error;
        // Add post to repository
        await contentRepository.UpdateAsync(post);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}