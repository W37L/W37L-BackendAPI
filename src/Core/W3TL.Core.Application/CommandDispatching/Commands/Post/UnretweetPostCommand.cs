using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.Post;

public class UnretweetPostCommand : Command<PostId>, ICommand<UnretweetPostCommand> {
    private UnretweetPostCommand(PostId postId, UserID unretweeterId) : base(postId) {
        UnretweeterId = unretweeterId;
    }

    public UserID UnretweeterId { get; }

    public static int ParametersCount { get; } = 2;

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