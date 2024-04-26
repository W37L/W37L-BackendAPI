using Microsoft.Extensions.DependencyInjection;
using Persistance.PostPersistance;
using Persistence.UserPersistence;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.UnitOfWork;

namespace Persistance.Extensions;

public static class RepositoryExtensions {
    public static void RegisterRepositories(this IServiceCollection services) {
        //Register Repositories
        services.AddScoped<IContentRepository, PostRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
    }
}