using Microsoft.Extensions.DependencyInjection;
using QueryContracts.QueryDispatching;

namespace Queries.Extensions;

public static class DispatcherExtension {
    
    ///<summary>
    /// Extension method to register the query dispatcher in the dependency injection container.
    ///</summary>
    ///<param name="serviceCollection">The collection of services to which the query dispatcher will be added.</param>
    public static void RegisterQueryDispatcher(this IServiceCollection serviceCollection) {
        serviceCollection.AddScoped<IQueryDispatcher, QueryDispatcher>();
    }
}