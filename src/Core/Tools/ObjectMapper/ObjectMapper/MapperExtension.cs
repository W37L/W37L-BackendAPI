using Microsoft.Extensions.DependencyInjection;
using ObjectMapper.DTO;
using ObjectMapper.Mappers;
using Persistence.UserPersistence;
using Persistence.UserPersistence.Firebase;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.User.Entity;

namespace ObjectMapper;

/// <summary>
///      Extension class for registering mappers in the dependency injection container.
/// </summary>
public static class MapperExtension {
    
    /// <summary>
    ///     Registers mapper implementations in the dependency injection container.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    public static void RegisterMappers(this IServiceCollection services) {
        services.AddScoped<IMapper, ConcreteObjectMapper>();

        services.AddScoped<IMappingConfig<User, UserDTO>, UserMapper>();
        services.AddScoped<IMappingConfig<UserDTO, User>, UserDomainMapper>();
        services.AddScoped<IMappingConfig<Interactions, InteractionsDTO>, InteractionsMapper>();
        services.AddScoped<IMappingConfig<ContentDTO, Post>, PostDomainMapper>();
        services.AddScoped<IMappingConfig<Post, ContentDTO>, PostMapper>();
        services.AddScoped<IMappingConfig<ContentDTO, Comment>, CommentDomainMapper>();
        services.AddScoped<IMappingConfig<Comment, ContentDTO>, CommentMapper>();
        services.AddScoped<IMappingConfig<ContentDTO, Content>, ContentDomainMapper>();
        services.AddScoped<IMappingConfig<Content, ContentDTO>, ContentMapper>();
    }
}