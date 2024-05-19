using W3TL.Core.Application.CommandDispatching.Commands;

namespace W3TL.Core.Application.CommandDispatching.Dispatcher;

/// <summary>
/// Represents an interface for a command dispatcher.
/// </summary>
public interface ICommandDispatcher {
    
    /// <summary>
    /// Dispatches the provided command asynchronously.
    /// </summary>
    /// <param name="command">The command to be dispatched.</param>
    /// <returns>A task representing the asynchronous operation, yielding a result indicating the success or failure of the command execution.</returns>
    Task<Result> DispatchAsync(Command command);
}