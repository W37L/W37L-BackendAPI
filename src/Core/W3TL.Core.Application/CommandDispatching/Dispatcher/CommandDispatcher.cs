using W3TL.Core.Application.CommandDispatching.Commands;

namespace W3TL.Core.Application.CommandDispatching.Dispatcher;

public class CommandDispatcher : ICommandDispatcher {
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public async Task<Result> DispatchAsync(Command command) {
        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);
        dynamic handler = _serviceProvider.GetService(handlerType) ?? throw new InvalidOperationException();
        return await handler.HandleAsync((dynamic)command);
    }
}