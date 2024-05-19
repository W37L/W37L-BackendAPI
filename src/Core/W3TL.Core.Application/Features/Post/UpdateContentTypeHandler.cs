using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.Post;

/// <summary>
/// Handles the command for updating the content type of a post.
/// </summary>
public class UpdateContentTypeHandler(
    IContentRepository contentRepository, 
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateContentTypeCommand> {
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateContentTypeHandler"/> class.
    /// </summary>
    /// <param name="contentRepository">The repository for accessing content.</param>
    /// <param name="unitOfWork">The unit of work for managing transactions.</param>
    public async Task<Result> HandleAsync(UpdateContentTypeCommand command) {
        // Search for post by id
        var result = await contentRepository.GetByIdAsync(command.Id);
        if (result.IsFailure)
            return Error.ContentNotFound;

        // Update post
        if (result.Payload is not global::Post post)
            return Error.ContentNotFound;

        var action = post.UpdateContentType(command.MediaType);

        if (action.IsFailure)
            return action.Error;

        // Add post to repository
        await contentRepository.UpdateAsync(post);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}