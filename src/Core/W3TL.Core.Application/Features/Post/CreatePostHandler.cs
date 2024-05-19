using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.Post;

/// <summary>
/// Handles the command for creating a new post.
/// </summary>
public class CreatePostHandler(
    IContentRepository contentRepository,
    IUnitOfWork unitOfWork,
    IUserRepository userRepository) : ICommandHandler<CreatePostCommand> {
    
    /// <summary>
    /// Handles the command to create a new post asynchronously.
    /// </summary>
    /// <param name="command">The command to create a new post.</param>
    /// <returns>A result indicating the success or failure of the operation.</returns>
    public async Task<Result> HandleAsync(CreatePostCommand command) {
        // Search for post by id
        var result = await contentRepository.GetByIdAsync(command.Id);
        if (result.IsSuccess)
            return Error.PostAlreadyRegistered;

        //Search for user by id
        var creator = await userRepository.GetByIdAsync(command.CreatorId);
        if (creator.IsFailure)
            return Error.UserNotFound;

        // if there is a parent post, search for it
        Content parent = null;
        if (command.ParentPostId != null) {
            var parentPost = await contentRepository.GetByIdAsync(command.ParentPostId);
            parent = parentPost.Payload;
            if (parentPost.IsFailure)
                return Error.ParentPostNotFound;
        }

        // Create post
        var post = global::Post.Create(command.Id, command.ContentTweet, creator.Payload, command.Signature,
            command.PostType, command.MediaUrl, command.MediaType, parent);

        // var post = global::Post.Create(command.ContentTweet, creator.Payload, command.Signature, command.PostType, command.MediaUrl, command.MediaType, parent);

        if (post.IsFailure)
            return post.Error;

        // Add post to repository
        await contentRepository.AddAsync(post.Payload);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}