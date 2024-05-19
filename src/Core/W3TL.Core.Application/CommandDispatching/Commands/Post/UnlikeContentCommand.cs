using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.Post;

/// <summary>
/// Represents a command to unlike content (e.g., a post).
/// </summary>
public class UnlikeContentCommand : Command<PostId>, ICommand<UnlikeContentCommand> {
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UnlikeContentCommand"/> class.
    /// </summary>
    /// <param name="postId">The ID of the post to be unliked.</param>
    /// <param name="unlikerId">The ID of the user who is unliking the post.</param>
    private UnlikeContentCommand(PostId postId, UserID unlikerId) : base(postId) {
        UnlikerId = unlikerId;
    }

    public UserID UnlikerId { get; }

    public static int ParametersCount { get; } = 2;

    /// <summary>
    /// Creates a new <see cref="UnlikeContentCommand"/> instance if the provided arguments are valid.
    /// </summary>
    /// <param name="args">The parameters required to create the command, typically a post ID and a user ID.</param>
    /// <returns>A result containing either the created command or an error.</returns>
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