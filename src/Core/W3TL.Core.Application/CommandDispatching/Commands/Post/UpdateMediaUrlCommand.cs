using W3TL.Core.Domain.Agregates.Post.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.Post;

public class UpdateMediaUrlCommand : Command<PostId>, ICommand<UpdateMediaUrlCommand> {
    private UpdateMediaUrlCommand(PostId postId, MediaUrl mediaUrl) : base(postId) {
        MediaUrl = mediaUrl;
    }

    public MediaUrl MediaUrl { get; }

    public static int ParametersCount { get; } = 2;

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