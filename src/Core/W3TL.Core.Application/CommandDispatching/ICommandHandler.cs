using W3TL.Core.Application.CommandDispatching.Commands;

namespace W3TL.Core.Application.CommandDispatching;

public interface ICommandHandler<in TCommand>
    where TCommand : Command {
    Task<Result> HandleAsync(TCommand command);
}