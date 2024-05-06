using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.Post;

public class HighLighPostCommand : Command<PostId>, ICommand<HighLighPostCommand> {
    private HighLighPostCommand(PostId postId, UserID highlighterId) : base(postId) {
        HighlighterId = highlighterId;
    }

    public UserID HighlighterId { get; }

    public static int ParametersCount { get; } = 2;

    public static Result<HighLighPostCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
            return Error.InvalidCommand;

        var errors = new HashSet<Error>();

        var postIdResult = PostId.Create(args[0].ToString())
            .OnFailure(error => errors.Add(error));

        var highlighterIdResult = UserID.Create(args[1].ToString())
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.CompileErrors(errors);

        return new HighLighPostCommand(postIdResult.Payload, highlighterIdResult.Payload);
    }
}