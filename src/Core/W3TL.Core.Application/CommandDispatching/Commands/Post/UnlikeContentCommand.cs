using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.Post;

public class UnlikeContentCommand : Command<PostId>, ICommand<UnlikeContentCommand> {
    private UnlikeContentCommand(PostId postId, UserID unlikerId) : base(postId) {
        UnlikerId = unlikerId;
    }

    public UserID UnlikerId { get; }

    public static int ParametersCount { get; } = 2;

    public static Result<UnlikeContentCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
            return Error.WrongNumberOfParameters;

        var errors = new HashSet<Error>();

        var postIdResult = PostId.Create(args[0].ToString())
            .OnFailure(error => errors.Add(error));

        var unlikerIdResult = UserID.Create(args[1].ToString())
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.CompileErrors(errors);

        return new UnlikeContentCommand(postIdResult.Payload, unlikerIdResult.Payload);
    }
}