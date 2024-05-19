using W3TL.Core.Application.CommandDispatching.Commands;

namespace W3TL.Core.Application.CommandDispatching.Dispatcher;

/// <summary>
/// Represents a dispatcher for handling commands.
/// </summary>
public class CommandDispatcher : ICommandDispatcher {
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandDispatcher"/> class with the specified service provider.
    /// </summary>
    /// <param name="serviceProvider">The service provider used for resolving command handlers.</param>
    public CommandDispatcher(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Dispatches the provided command asynchronously.
    /// </summary>
    /// <param name="command">The command to be dispatched.</param>
    /// <returns>A result indicating the success or failure of the command execution.</returns>
    public async Task<Result> DispatchAsync(Command command) {
        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);
        dynamic handler = _serviceProvider.GetService(handlerType) ?? throw new InvalidOperationException();
        return await handler.HandleAsync((dynamic)command);
    }
}