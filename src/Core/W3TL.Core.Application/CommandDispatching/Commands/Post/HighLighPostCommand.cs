using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.Post;

/// <summary>
/// Represents a command to highlight a post.
/// </summary>
public class HighLighPostCommand : Command<PostId>, ICommand<HighLighPostCommand> {
    
    /// <summary>
    /// Initializes a new instance of the <see cref="HighLighPostCommand"/> class.
    /// </summary>
    /// <param name="postId">The ID of the post to be highlighted.</param>
    /// <param name="highlighterId">The ID of the user who is highlighting the post.</param>
    private HighLighPostCommand(PostId postId, UserID highlighterId) : base(postId) {
        HighlighterId = highlighterId;
    }

    public UserID HighlighterId { get; }

    public static int ParametersCount { get; } = 2;

    /// <summary>
    /// Creates a new <see cref="HighLighPostCommand"/> instance if the provided arguments are valid.
    /// </summary>
    /// <param name="args">The parameters required to create the command, typically a post ID and a user ID.</param>
    /// <returns>A result containing either the created command or an error.</returns>
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