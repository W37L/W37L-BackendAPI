using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.Post;

public class UnlikeContentHandler(
    IContentRepository contentRepository,
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    IInteractionRepository interactionRepository) : ICommandHandler<UnlikeContentCommand> {
    public async Task<Result> HandleAsync(UnlikeContentCommand command) {
        // Search for post by id
        var unliker = await userRepository.GetByIdAsync(command.UnlikerId);
        if (unliker.IsFailure)
            return Error.UserNotFound;

        var result = await contentRepository.GetByFullIdAsync(command.Id, unliker.Payload);

        if (result.IsFailure)
            return Error.PostNotFound;

        if (result.Payload is global::Post post) {
            // Unlike post
            var unlike = post.Unlike(unliker.Payload);

            if (unlike.IsFailure)
                return unlike.Error;

            // Add unlike to repository
            await contentRepository.UpdateAsync(post);
            await interactionRepository.UnlikeTweetAsync(unliker.Payload.Id.Value, post.Id.Value);
            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        return Error.PostNotFound;
    }
}