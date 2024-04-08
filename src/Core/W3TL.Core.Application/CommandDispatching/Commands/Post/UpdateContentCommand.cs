using W3TL.Core.Domain.Agregates.Post.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.Post;

public class UpdateContentCommand : Command<PostId>, ICommand<UpdateContentCommand> {
    private UpdateContentCommand(PostId postId, TheString contentTweet, Signature signature) : base(postId) {
        ContentTweet = contentTweet;
        Signature = signature;
    }

    public TheString ContentTweet { get; }
    public Signature Signature { get; }

    public static int ParametersCount { get; } = 3;

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