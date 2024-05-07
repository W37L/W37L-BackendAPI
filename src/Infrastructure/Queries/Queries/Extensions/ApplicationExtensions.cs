using Microsoft.Extensions.DependencyInjection;
using Queries.Queries.Comments;
using Queries.Queries.Post;
using Queries.Queries.User;
using QueryContracts.Contracts;
using QueryContracts.Queries;
using QueryContracts.Queries.Comments;
using QueryContracts.Queries.Users;

namespace Queries.Extensions;

public static class ApplicationExtensions {
    public static void RegisterQueries(this IServiceCollection services) {
        // Posts
        services.AddScoped<IQueryHandler<GetAllPostQuery.Query, GetAllPostQuery.Answer>, GetAllPostHandler>();
        services.AddScoped<IQueryHandler<GetPostByIdQuery.Query, GetPostByIdQuery.Answer>, GetPostsQueryHandler>();
        services
            .AddScoped<IQueryHandler<GetPostsByUserIdQuery.Query, GetPostsByUserIdQuery.Answer>,
                GetPostsByUserIdHandler>();
        services
            .AddScoped<IQueryHandler<GetPostsByUsernameQuery.Query, GetPostsByUsernameQuery.Answer>,
                GetPostsByUsernameHandler>();

        // Comments
        services
            .AddScoped<IQueryHandler<GetAllCommentsByPostIdQuery.Query, GetAllCommentsByPostIdQuery.Answer>,
                GetAllCommentsByPostIdHandler>();
        services
            .AddScoped<IQueryHandler<GetHowManyCommentsByPostIdQuery.Query, GetHowManyCommentsByPostIdQuery.Answer>,
                GetHowManyCommentsByPostIdHandler>();
        services
            .AddScoped<IQueryHandler<GetCommentByIdQuery.Query, GetCommentByIdQuery.Answer>, GetCommentByIdHandler>();
        services
            .AddScoped<IQueryHandler<GetAllCommentsByUserIdQuery.Query, GetAllCommentsByUserIdQuery.Answer>,
                GetAllCommentByUserIdHandler>();

        // UserRelationships
        services.AddScoped<IQueryHandler<GetFollowingQuery.Query, GetFollowingQuery.Answer>, GetFollowingHandler>();
        services.AddScoped<IQueryHandler<GetFollowersQuery.Query, GetFollowersQuery.Answer>, GetFollowersHandler>();
        services.AddScoped<IQueryHandler<GetRelationsQuery.Query, GetRelationsQuery.Answer>, GetRelationsHandler>();

        //users
        services.AddScoped<IQueryHandler<GetAllUsersQuery.Query, GetAllUsersQuery.Answer>, GetAllUsersHandler>();
        services.AddScoped<IQueryHandler<GetUserByIdQuery.Query, GetUserByIdQuery.Answer>, GetUserByIdHandler>();
        services
            .AddScoped<IQueryHandler<GetUserByUsernameQuery.Query, GetUserByUsernameQuery.Answer>,
                GetUserByUsernameHandler>();
    }
}