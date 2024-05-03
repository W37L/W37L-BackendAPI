using Microsoft.Extensions.DependencyInjection;
using QueryContracts.QueryDispatching;

namespace Queries.Extensions;

public static class DispatcherExtension {
    public static void RegisterQueryDispatcher(this IServiceCollection serviceCollection) {
        serviceCollection.AddScoped<IQueryDispatcher, QueryDispatcher>();
    }
}