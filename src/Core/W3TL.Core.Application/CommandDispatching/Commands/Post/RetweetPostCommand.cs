using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.Post;

public class RetweetPostCommand : Command<PostId>, ICommand<RetweetPostCommand> {
    private RetweetPostCommand(PostId postId, UserID retweeterId) : base(postId) {
        RetweeterId = retweeterId;
    }

    public UserID RetweeterId { get; }

    public static int ParametersCount { get; } = 2;

    public static Result<RetweetPostCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
            return Error.InvalidCommand;

        var errors = new HashSet<Error>();

        var postIdResult = PostId.Create(args[0].ToString())
            .OnFailure(error => errors.Add(error));

        var retweeterIdResult = UserID.Create(args[1].ToString())
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.CompileErrors(errors);

        return new RetweetPostCommand(postIdResult.Payload, retweeterIdResult.Payload);
    }
}