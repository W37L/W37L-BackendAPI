namespace W3TL.Core.Application.CommandDispatching.Commands;

/// <summary>
/// Represents a generic interface for commands.
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
public interface ICommand<TCommand> where TCommand : Command {
    
    /// <summary>
    /// Gets the number of parameters required to create the command.
    /// </summary>
    public static abstract int ParametersCount { get; }
    
    /// <summary>
    /// Creates a command instance based on the provided arguments.
    /// </summary>
    /// <param name="args">The arguments used to create the command.</param>
    /// <returns>A result containing either the created command or an error.</returns>
    public static abstract Result<TCommand> Create(params object[] args);
}