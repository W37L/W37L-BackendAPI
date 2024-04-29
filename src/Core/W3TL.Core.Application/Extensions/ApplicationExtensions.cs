using Microsoft.Extensions.DependencyInjection;
using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.Features.Post;
using W3TL.Core.Application.Features.User;

namespace W3TL.Core.Application.Extensions;

public static class ApplicationExtensions {
    public static void RegisterHandlers(this IServiceCollection services) {
        //Post
        services.AddScoped<ICommandHandler<CreatePostCommand>, CreatePostHandler>();
        services.AddScoped<ICommandHandler<LikeContentCommand>, LikeContentHandler>();
        services.AddScoped<ICommandHandler<UnlikeContentCommand>, UnlikeContentHandler>();

        //User
        services.AddScoped<ICommandHandler<CreateUserCommand>, CreateUserHandler>();
        services.AddScoped<ICommandHandler<UpdateUserCommand>, UpdateUserHandler>();
        services.AddScoped<ICommandHandler<UpdateAvatarUserCommand>, UpdateAvatarHandler>();
        services.AddScoped<ICommandHandler<UpdateProfileBannerCommand>, UpdateProfileBannerHandler>();
        services.AddScoped<ICommandHandler<FollowAUserCommand>, FollowAUserHanldler>();
        services.AddScoped<ICommandHandler<UnfollowAUserCommand>, UnfollowAUserHandler>();
    }
}