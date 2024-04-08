using W3TL.Core.Application.CommandDispatching.Commands;

namespace W3TL.Core.Application.CommandDispatching.Dispatcher;

public interface ICommandDispatcher {
    Task<Result> DispatchAsync(Command command);
}