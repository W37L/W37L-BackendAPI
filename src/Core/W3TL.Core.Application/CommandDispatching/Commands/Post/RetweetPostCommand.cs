using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.Post;

/// <summary>
/// Represents a command to retweet a post.
/// </summary>
public class RetweetPostCommand : Command<PostId>, ICommand<RetweetPostCommand> {
    
    /// <summary>
    /// Initializes a new instance of the <see cref="RetweetPostCommand"/> class.
    /// </summary>
    /// <param name="postId">The ID of the post to be retweeted.</param>
    /// <param name="retweeterId">The ID of the user who is retweeting the post.</param>
    private RetweetPostCommand(PostId postId, UserID retweeterId) : base(postId) {
        RetweeterId = retweeterId;
    }

    public UserID RetweeterId { get; }

    public static int ParametersCount { get; } = 2;

    /// <summary>
    /// Creates a new <see cref="RetweetPostCommand"/> instance if the provided arguments are valid.
    /// </summary>
    /// <param name="args">The parameters required to create the command, typically a post ID and a user ID.</param>
    /// <returns>A result containing either the created command or an error.</returns>
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