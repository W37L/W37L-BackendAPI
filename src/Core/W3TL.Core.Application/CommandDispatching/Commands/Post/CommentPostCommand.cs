using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.Post;

/// <summary>
///     Represents a command to comment on a post.
/// </summary>
public class CommentPostCommand : Command<CommentId>, ICommand<CommentPostCommand> {
    
    /// <summary>
    ///     Initializes a new instance of the CommentPostCommand class.
    /// </summary>
    /// <param name="postId">The ID of the post to be commented on.</param>
    /// <param name="content">The content of the comment.</param>
    /// <param name="creatorId">The ID of the user creating the comment.</param>
    /// <param name="signature">The signature associated with the comment.</param>
    /// <param name="parentPostId">The ID of the parent post (optional for nested comments).</param>
    private CommentPostCommand(CommentId postId, TheString content, UserID creatorId, Signature signature,
        PostId parentPostId) : base(postId) {
        Content = content;
        CreatorId = creatorId;
        Signature = signature;
        ParentPostId = parentPostId;
    }

    public TheString Content { get; }
    public UserID CreatorId { get; }
    public Signature Signature { get; }
    public PostId ParentPostId { get; }

    public static int ParametersCount { get; } = 5;

    /// <summary>
    ///     Attempts to create a new CommentPostCommand instance from the provided arguments.
    /// </summary>
    /// <param name="args">An array of objects representing the command parameters.</param>
    /// <returns>A Result object containing either a new CommentPostCommand instance or an error message if creation fails.</returns>
    public static Result<CommentPostCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
            return Error.WrongNumberOfParameters;

        var errors = new HashSet<Error>();

        // Attempt to create each property value from its corresponding argument and collect any errors.
        var commentIdResult = (args[0] == null) || string.IsNullOrWhiteSpace(args[0]?.ToString())
            ? CommentId.Generate()
            : CommentId.Create(args[0].ToString())
                .OnFailure(error => errors.Add(error));

        var contentResult = TheString.Create(args[1].ToString())
            .OnFailure(error => errors.Add(error));

        var creatorIdResult = UserID.Create(args[2].ToString())
            .OnFailure(error => errors.Add(error));

        var signatureResult = Signature.Create(args[3].ToString())
            .OnFailure(error => errors.Add(error));

        var parentPostIdResult = PostId.Create(args[4].ToString())
            .OnFailure(error => errors.Add(error));


        if (errors.Any())
            return Error.CompileErrors(errors);

        return new CommentPostCommand(commentIdResult.Payload, contentResult.Payload, creatorIdResult.Payload,
            signatureResult.Payload, parentPostIdResult.Payload);
    }
}