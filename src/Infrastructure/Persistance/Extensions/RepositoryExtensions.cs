using Microsoft.Extensions.DependencyInjection;
using Persistance.PostPersistance;
using Persistance.UserPersistance;
using Persistence.UserPersistence;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace Persistance.Extensions;

/// <summary>
/// Provides extension methods for registering repositories in the service collection.
/// </summary>
public static class RepositoryExtensions {
    /// <summary>
    /// Registers repositories in the specified service collection.
    /// </summary>
    /// <param name="services">The service collection to register the repositories with.</param>
    public static void RegisterRepositories(this IServiceCollection services) {
        // Register Repositories
        services.AddScoped<IContentRepository, PostRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IInteractionRepository, InteractionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
    }
}