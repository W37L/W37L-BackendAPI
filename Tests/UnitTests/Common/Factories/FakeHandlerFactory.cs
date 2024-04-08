using Microsoft.Extensions.DependencyInjection;
using UnitTests.Fakes.Handlers;
using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands;

public static class FakeHandlerFactory {
    private static readonly Dictionary<Type, object> Handlers = new();

    public static ICommandHandler<TCommand> CreateHandler<TCommand>() where TCommand : Command {
        var handlerType = typeof(GenericFakeHandler<TCommand>);
        if (!Handlers.TryGetValue(handlerType, out var handler)) {
            handler = Activator.CreateInstance(handlerType);
            Handlers[handlerType] = handler;
        }

        return (ICommandHandler<TCommand>) handler;
    }

    public static void RegisterHandlers(IServiceCollection services) {
        foreach (var handlerEntry in Handlers) services.AddScoped(handlerEntry.Key, _ => handlerEntry.Value);
    }

    public static void ClearHandlers() {
        Handlers.Clear();
    }
}