using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.User;

public class CommentPostHandler(IContentRepository contentRepository, IUnitOfWork unitOfWork, IUserRepository userRepository) : ICommandHandler<CommentPostCommand> {
    public async Task<Result> HandleAsync(CommentPostCommand command) {
        // Search for post by id
        var result = await contentRepository.GetByIdAsync(command.Id);

        if (result.IsFailure)
            return Error.ParentPostNotFound;

        if (result.Payload is global::Post parent) {
            // Search for user by id
            var creator = await userRepository.GetByIdAsync(command.CreatorId);
            if (creator.IsFailure)
                return Error.UserNotFound;

            // Create comment
            var comment = Comment.Create(command.Content, creator.Payload, command.Signature, parent)
                .OnSuccess(comment => parent.AddComment(comment));

            if (comment.IsFailure)
                return comment.Error;

            // Add comment to repository
            await contentRepository.AddAsync(comment.Payload);
            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        return Error.ParentPostNotFound;
    }
}