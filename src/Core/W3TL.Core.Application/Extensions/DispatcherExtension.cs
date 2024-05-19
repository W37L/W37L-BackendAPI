using Microsoft.Extensions.DependencyInjection;
using W3TL.Core.Application.CommandDispatching.Dispatcher;

namespace W3TL.Core.Application.Extensions;

/// <summary>
/// Provides an extension method for registering the command dispatcher with the service collection.
/// </summary>
public static class DispatcherExtension {
    
    /// <summary>
    /// Registers the command dispatcher with the specified service collection.
    /// </summary>
    /// <param name="services">The service collection to register the command dispatcher with.</param>
    public static void RegisterCommandDispatcher(this IServiceCollection services) {
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
    }
}