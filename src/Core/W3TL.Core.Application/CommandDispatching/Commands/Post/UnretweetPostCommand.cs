using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.Post;

/// <summary>
/// Represents a command to unretweet content (e.g., a post).
/// </summary>
public class UnretweetPostCommand : Command<PostId>, ICommand<UnretweetPostCommand> {
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UnretweetPostCommand"/> class.
    /// </summary>
    /// <param name="postId">The ID of the post to be unretweeted.</param>
    /// <param name="unretweeterId">The ID of the user who is unretweeting the post.</param>
    private UnretweetPostCommand(PostId postId, UserID unretweeterId) : base(postId) {
        UnretweeterId = unretweeterId;
    }

    /// <summary>
    ///  The ID of the user who is unretweeting the post.
    /// </summary>
    public UserID UnretweeterId { get; }

    /// <summary>
    ///   The number of parameters required to create a <see cref="UnretweetPostCommand"/>.
    /// </summary>
    public static int ParametersCount { get; } = 2;

    /// <summary>
    /// Creates a new <see cref="UnretweetPostCommand"/> instance if the provided arguments are valid.
    /// </summary>
    /// <param name="args">The parameters required to create the command, typically a post ID and a user ID.</param>
    /// <returns>A result containing either the created command or an error.</returns>
    public static Result<UnretweetPostCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
            return Error.InvalidCommand;

        var errors = new HashSet<Error>();

        var postIdResult = PostId.Create(args[0].ToString())
            .OnFailure(error => errors.Add(error));

        var unretweeterIdResult = UserID.Create(args[1].ToString())
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.CompileErrors(errors);

        return new UnretweetPostCommand(postIdResult.Payload, unretweeterIdResult.Payload);
    }
}