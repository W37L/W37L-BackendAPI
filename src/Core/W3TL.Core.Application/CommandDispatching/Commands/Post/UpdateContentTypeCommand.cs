using W3TL.Core.Domain.Agregates.Post.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.Post;

/// <summary>
/// Represents a command to update the content type of a post.
/// </summary>
public class UpdateContentTypeCommand : Command<PostId>, ICommand<UpdateContentTypeCommand> {
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateContentTypeCommand"/> class.
    /// </summary>
    /// <param name="postId">The ID of the post to be updated.</param>
    /// <param name="mediaType">The new media type of the post.</param>
    private UpdateContentTypeCommand(PostId postId, MediaType mediaType) : base(postId) {
        MediaType = mediaType;
    }

    public MediaType MediaType { get; }

    public static int ParametersCount { get; } = 2;

    /// <summary>
    /// Creates a new <see cref="UpdateContentTypeCommand"/> instance if the provided arguments are valid.
    /// </summary>
    /// <param name="args">The parameters required to create the command, typically a post ID and a media type.</param>
    /// <returns>A result containing either the created command or an error.</returns>
    public static Result<UpdateContentTypeCommand> Create(params object[] args) {
        if (args.Length != ParametersCount) return Error.WrongNumberOfParameters;

        var errors = new HashSet<Error>();

        var postIdResult = PostId.Create(args[0].ToString());
        if (postIdResult.IsFailure) errors.Add(postIdResult.Error);

        if (!Enum.TryParse<MediaType>(args[1].ToString(), true, out var mediaType)) {
            errors.Add(Error.InvalidPostType);
        }

        if (errors.Any()) {
            return Error.CompileErrors(errors);
        }

        return new UpdateContentTypeCommand(postIdResult.Payload, mediaType);
    }
}