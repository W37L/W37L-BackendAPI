using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.Post;

public class CommentPostCommand : Command<CommentId>, ICommand<CommentPostCommand> {
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

    public static Result<CommentPostCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
            return Error.WrongNumberOfParameters;

        var errors = new HashSet<Error>();

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