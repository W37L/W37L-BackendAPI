using W3TL.Core.Domain.Agregates.Post.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.Post;

/// <summary>
/// Represents a command to update the content of a post.
/// </summary>
public class UpdateContentCommand : Command<PostId>, ICommand<UpdateContentCommand> {
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateContentCommand"/> class.
    /// </summary>
    /// <param name="postId">The ID of the post to be updated.</param>
    /// <param name="contentTweet">The new content of the post.</param>
    /// <param name="signature">The signature for verification.</param>
    private UpdateContentCommand(PostId postId, TheString contentTweet, Signature signature) : base(postId) {
        ContentTweet = contentTweet;
        Signature = signature;
    }

    /// <summary>
    ///  The new content of the post to be updated.
    /// </summary>
    public TheString ContentTweet { get; }
    
    /// <summary>
    ///  The signature for verification.
    /// </summary>
    public Signature Signature { get; }

    /// <summary>
    ///  The number of parameters required to create the command.
    /// </summary>
    public static int ParametersCount { get; } = 3;

    /// <summary>
    /// Creates a new <see cref="UpdateContentCommand"/> instance if the provided arguments are valid.
    /// </summary>
    /// <param name="args">The parameters required to create the command, typically a post ID, new content, and a signature.</param>
    /// <returns>A result containing either the created command or an error.</returns>
    public static Result<UpdateContentCommand> Create(params object[] args) {
        if (args.Length != ParametersCount) return Error.WrongNumberOfParameters;

        var errors = new HashSet<Error>();

        var postIdResult = PostId.Create(args[0].ToString());
        if (postIdResult.IsFailure) errors.Add(postIdResult.Error);

        var contentTweetResult = TheString.Create(args[1].ToString());
        if (contentTweetResult.IsFailure) errors.Add(contentTweetResult.Error);

        var signatureResult = Signature.Create(args[2].ToString());
        if (signatureResult.IsFailure) errors.Add(signatureResult.Error);

        if (errors.Any()) {
            return Error.CompileErrors(errors);
        }

        return new UpdateContentCommand(postIdResult.Payload, contentTweetResult.Payload, signatureResult.Payload);
    }
}