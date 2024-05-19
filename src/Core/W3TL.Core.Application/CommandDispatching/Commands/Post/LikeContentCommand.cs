using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.Post;

/// <summary>
/// Represents a command to like content (e.g., a post).
/// </summary>
public class LikeContentCommand : Command<PostId>, ICommand<LikeContentCommand> {
    
    /// <summary>
    /// Initializes a new instance of the <see cref="LikeContentCommand"/> class.
    /// </summary>
    /// <param name="postId">The ID of the post to be liked.</param>
    /// <param name="likerId">The ID of the user who is liking the post.</param>
    private LikeContentCommand(PostId postId, UserID likerId) : base(postId) {
        LikerId = likerId;
    }

    /// <summary>
    ///  The ID of the user who is liking the post.
    /// </summary>
    public UserID LikerId { get; }

    /// <summary>
    ///  The number of parameters required to create a <see cref="LikeContentCommand"/>.
    /// </summary>
    public static int ParametersCount { get; } = 2;

    /// <summary>
    /// Creates a new <see cref="LikeContentCommand"/> instance if the provided arguments are valid.
    /// </summary>
    /// <param name="args">The parameters required to create the command, typically a post ID and a user ID.</param>
    /// <returns>A result containing either the created command or an error.</returns>
    public static Result<LikeContentCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
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