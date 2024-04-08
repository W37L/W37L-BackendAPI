using W3TL.Core.Domain.Agregates.Post.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.Post;

public class UpdateContentTypeCommand : Command<PostId>, ICommand<UpdateContentTypeCommand> {
    private UpdateContentTypeCommand(PostId postId, MediaType mediaType) : base(postId) {
        MediaType = mediaType;
    }

    public MediaType MediaType { get; }

    public static int ParametersCount { get; } = 2;

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