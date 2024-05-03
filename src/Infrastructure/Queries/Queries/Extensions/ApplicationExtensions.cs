using Microsoft.Extensions.DependencyInjection;
using Queries.Queries.Post;
using Queries.Queries.User;
using QueryContracts.Contracts;
using QueryContracts.Queries;
using QueryContracts.Queries.Users;

namespace Queries.Extensions;

public static class ApplicationExtensions {
    public static void RegisterQueries(this IServiceCollection services) {
        services.AddScoped<IQueryHandler<GetAllPostQuery.Query, GetAllPostQuery.Answer>, GetAllPostHandler>();
        services.AddScoped<IQueryHandler<GetPostByIdQuery.Query, GetPostByIdQuery.Answer>, GetPostsQueryHandler>();
        services
            .AddScoped<IQueryHandler<GetPostsByUserIdQuery.Query, GetPostsByUserIdQuery.Answer>,
                GetPostsByUserIdHandler>();
        services
            .AddScoped<IQueryHandler<GetPostsByUsernameQuery.Query, GetPostsByUsernameQuery.Answer>,
                GetPostsByUsernameHandler>();
        services.AddScoped<IQueryHandler<GetUserByIdQuery.Query, GetUserByIdQuery.Answer>, GetUserByIdHandler>();
        services
            .AddScoped<IQueryHandler<GetUserByUsernameQuery.Query, GetUserByUsernameQuery.Answer>,
                GetUserByUsernameHandler>();
        services.AddScoped<IQueryHandler<GetAllUsersQuery.Query, GetAllUsersQuery.Answer>, GetAllUsersHandler>();
    }
}