using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.Post;

public class LikeContentCommand : Command<PostId>, ICommand<LikeContentCommand> {
    private LikeContentCommand(PostId postId, UserID likerId) : base(postId) {
        LikerId = likerId;
    }

    public UserID LikerId { get; }

    public static int ParametersCount { get; } = 2;

    public static Result<LikeContentCommand> Create(params object[] args) {
        if (args.Length < ParametersCount)
            return Error.InvalidCommand;

        var errors = new HashSet<Error>();

        var postIdResult = PostId.Create(args[0].ToString())
            .OnFailure(error => errors.Add(error));

        var likerIdResult = UserID.Create(args[1].ToString())
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.CompileErrors(errors);

        return new LikeContentCommand(postIdResult.Payload, likerIdResult.Payload);
    }
}