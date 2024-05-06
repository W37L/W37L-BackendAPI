using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace W3TL.Core.Application.Features.Post;

public class RetweetPostHandler : ICommandHandler<RetweetPostCommand> {
    private readonly IContentRepository contentRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IUserRepository userRepository;
    private IInteractionRepository interactionRepository;

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