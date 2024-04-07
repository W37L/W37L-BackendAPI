using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.Post;

public class UpdateMediaUrlHandler(IContentRepository contentRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateMediaUrlCommand> {
    public async Task<Result> HandleAsync(UpdateMediaUrlCommand command) {
        // Search for post by id
        var result = await contentRepository.GetByIdAsync(command.Id);
        if (result.IsFailure)
            return Error.ContentNotFound;

        // Update post
        if (result.Payload is not global::Post post)
            return Error.ContentNotFound;

        var action = post.UpdateMediaUrl(command.MediaUrl);
        if (action.IsFailure)
            return action.Error;

        // Add post to repository
        await contentRepository.UpdateAsync(post);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}