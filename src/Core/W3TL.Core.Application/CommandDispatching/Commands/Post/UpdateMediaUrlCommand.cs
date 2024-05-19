using W3TL.Core.Domain.Agregates.Post.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.Post;

/// <summary>
/// Represents a command to update the media URL of a post.
/// </summary>
public class UpdateMediaUrlCommand : Command<PostId>, ICommand<UpdateMediaUrlCommand> {
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateMediaUrlCommand"/> class.
    /// </summary>
    /// <param name="postId">The ID of the post to be updated.</param>
    /// <param name="mediaUrl">The new media URL of the post.</param>
    private UpdateMediaUrlCommand(PostId postId, MediaUrl mediaUrl) : base(postId) {
        MediaUrl = mediaUrl;
    }

    public MediaUrl MediaUrl { get; }

    public static int ParametersCount { get; } = 2;

    /// <summary>
    /// Creates a new <see cref="UpdateMediaUrlCommand"/> instance if the provided arguments are valid.
    /// </summary>
    /// <param name="args">The parameters required to create the command, typically a post ID and a media URL.</param>
    /// <returns>A result containing either the created command or an error.</returns>
    public static Result<UpdateMediaUrlCommand> Create(params object[] args) {
        if (args.Length != ParametersCount) return Error.WrongNumberOfParameters;

        var errors = new HashSet<Error>();

        var postIdResult = PostId.Create(args[0].ToString());
        if (postIdResult.IsFailure) errors.Add(postIdResult.Error);

        var mediaUrlResult = MediaUrl.Create(args[1].ToString());
        if (mediaUrlResult.IsFailure) errors.Add(mediaUrlResult.Error);

        if (errors.Any()) {
            return Error.CompileErrors(errors);
        }

        return new UpdateMediaUrlCommand(postIdResult.Payload, mediaUrlResult.Payload);
    }
}