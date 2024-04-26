using Microsoft.Extensions.DependencyInjection;
using W3TL.Core.Application.CommandDispatching.Dispatcher;

namespace W3TL.Core.Application.Extensions;

public static class DispatcherExtension {
    public static void RegisterCommandDispatcher(this IServiceCollection services) {
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
    }
}