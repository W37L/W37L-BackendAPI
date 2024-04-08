using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Application.CommandDispatching.Commands.User;

namespace UnitTests.Fakes.Handlers;

public abstract class GenericFakeHandler<TCommand> : ICommandHandler<TCommand> where TCommand : Command {
    public bool WasCalled { get; private set; }
    public TCommand HandledCommand { get; private set; }

    public Task<Result> HandleAsync(TCommand command) {
        WasCalled = true;
        HandledCommand = command;
        return Task.FromResult(Result.Success());
    }

    // Post handlers
    public class CommentPostFakeHandler : GenericFakeHandler<CommentPostCommand> { }

    public class CreatePostFakeHandler : GenericFakeHandler<CreatePostCommand> { }

    public class LikeContentFakeHandler : GenericFakeHandler<LikeContentCommand> { }

    public class UnlikeContentFakeHandler : GenericFakeHandler<UnlikeContentCommand> { }

    public class UpdateContentFakeHandler : GenericFakeHandler<UpdateContentCommand> { }

    public class UpdateContentTypeFakeHandler : GenericFakeHandler<UpdateContentTypeCommand> { }

    public class UpdateMediaUrlFakeHandler : GenericFakeHandler<UpdateMediaUrlCommand> { }

    // User handlers
    public class CreateUserFakeHandler : GenericFakeHandler<CreateUserCommand> { }

    public class UpdateUserFakeHandler : GenericFakeHandler<UpdateUserCommand> { }

    public class BlockUserFakeHandler : GenericFakeHandler<BlockUserCommand> { }

    public class UnblockUserFakeHandler : GenericFakeHandler<UnblockUserCommand> { }

    public class FollowAUserFakeHandler : GenericFakeHandler<FollowAUserCommand> { }

    public class UnfollowAUserFakeHandler : GenericFakeHandler<UnfollowAUserCommand> { }

    public class UpdateAvatarUserFakeHandler : GenericFakeHandler<UpdateAvatarUserCommand> { }

    public class UpdateProfileBannerFakeHandler : GenericFakeHandler<UpdateProfileBannerCommand> { }

    public class UpdateUserCommandFakeHandler : GenericFakeHandler<UpdateUserCommand> { }
}