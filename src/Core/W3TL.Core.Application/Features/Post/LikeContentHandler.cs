using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.Post;

public class LikeContentHandler(IContentRepository contentRepository, IUnitOfWork unitOfWork, IUserRepository userRepository) : ICommandHandler<LikeContentCommand> {
    public async Task<Result> HandleAsync(LikeContentCommand command) {
        // Search for post by id
        var result = await contentRepository.GetByIdAsync(command.Id);

        if (result.IsFailure)
            return Error.PostNotFound;

        if (result.Payload is global::Post post) {
            // Search for user by id
            var liker = await userRepository.GetByIdAsync(command.LikerId);
            if (liker.IsFailure)
                return Error.UserNotFound;

            // Like post
            var like = post.Like(liker.Payload);

            if (like.IsFailure)
                return like.Error;

            // Add like to repository
            await contentRepository.UpdateAsync(post);
            await userRepository.UpdateAsync(liker.Payload);
            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        return Error.PostNotFound;
    }
}