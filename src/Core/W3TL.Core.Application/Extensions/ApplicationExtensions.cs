using Microsoft.Extensions.DependencyInjection;
using W3TL.Core.Application.CommandDispatching;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.Features.Post;
using W3TL.Core.Application.Features.User;

namespace W3TL.Core.Application.Extensions;

public static class ApplicationExtensions {
    public static void RegisterHandlers(this IServiceCollection services) {
        //Post
        services.AddScoped<ICommandHandler<CreatePostCommand>, CreatePostHandler>();

        //User
        services.AddScoped<ICommandHandler<CreateUserCommand>, CreateUserHandler>();
        services.AddScoped<ICommandHandler<UpdateUserCommand>, UpdateUserHandler>();
    }
}