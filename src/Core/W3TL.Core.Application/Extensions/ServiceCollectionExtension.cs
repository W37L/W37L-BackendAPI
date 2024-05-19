using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using W3TL.Core.Application.CommandDispatching;

/// <summary>
/// Registers command handlers from the specified assembly with the service collection.
/// </summary>
/// <param name="services">The service collection to register the command handlers with.</param>
/// <param name="assembly">The assembly containing the command handler types.</param>
/// <returns>The modified service collection.</returns>
public static class ServiceCollectionExtensions {
    public static IServiceCollection AddCommandHandlers(this IServiceCollection services, Assembly assembly) {
        var handlerInterfaceType = typeof(ICommandHandler<>);
        var handlerTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType))
            .ToList();

        foreach (var type in handlerTypes) {
            var interfaceType = type.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType);
            services.AddScoped(interfaceType, type);

            Console.WriteLine($"Registered command handler: {type.Name} with service type: {interfaceType.Name}");
        }

        return services;
    }
}