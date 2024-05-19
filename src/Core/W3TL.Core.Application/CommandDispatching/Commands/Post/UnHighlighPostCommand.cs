using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.Post;

/// <summary>
/// Represents a command to unhighlight a post.
/// </summary>
public class UnHighlighPostCommand : Command<PostId>, ICommand<UnHighlighPostCommand> {
    /// <summary>
    /// Initializes a new instance of the <see cref="UnHighlighPostCommand"/> class.
    /// </summary>
    /// <param name="postId">The ID of the post to be unhighlighted.</param>
    /// <param name="highlighterId">The ID of the user who is unhighlighting the post.</param>
    private UnHighlighPostCommand(PostId postId, UserID highlighterId) : base(postId) {
        HighlighterId = highlighterId;
    }

    /// <summary>
    ///  The ID of the user who is unhighlighting the post.
    /// </summary>
    public UserID HighlighterId { get; }

    /// <summary>
    ///  The number of parameters required to create a <see cref="UnHighlighPostCommand"/>.
    /// </summary>
    public static int ParametersCount { get; } = 2;

    /// <summary>
    /// Creates a new <see cref="UnHighlighPostCommand"/> instance if the provided arguments are valid.
    /// </summary>
    /// <param name="args">The parameters required to create the command, typically a post ID and a user ID.</param>
    /// <returns>A result containing either the created command or an error.</returns>
    public static Result<UnHighlighPostCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
            return Error.InvalidCommand;

        var errors = new HashSet<Error>();

        var postIdResult = PostId.Create(args[0].ToString())
            .OnFailure(error => errors.Add(error));

        var highlighterIdResult = UserID.Create(args[1].ToString())
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.CompileErrors(errors);

        return new UnHighlighPostCommand(postIdResult.Payload, highlighterIdResult.Payload);
    }
}