using W3TL.Core.Application.CommandDispatching.Commands;

namespace W3TL.Core.Application.CommandDispatching;

/// <summary>
/// Represents an interface for handling commands of a specific type.
/// </summary>
/// <typeparam name="TCommand">The type of the command to be handled.</typeparam>
public interface ICommandHandler<in TCommand> where TCommand : Command {
    
    /// <summary>
    /// Handles the specified command asynchronously.
    /// </summary>
    /// <param name="command">The command to be handled.</param>
    /// <returns>A task representing the asynchronous operation, yielding a result indicating the success or failure of the command execution.</returns>
    Task<Result> HandleAsync(TCommand command);
}