using Persistence.UserPersistence;

namespace ObjectMapper.Mappers;

/// <summary>
///   The UserMapper class is responsible for mapping instances of the User class to instances of the UserDTO class.
/// </summary>
public class UserMapper : IMappingConfig<User, UserDTO> {
    
    /// <summary>
    ///     Maps a User object to a UserDTO object.
    /// </summary>
    /// <param name="user">The User object to be mapped.</param>
    /// <returns>The mapped UserDTO object.</returns>
    public UserDTO Map(User user) {
        return new UserDTO {
            userId = user.Id.Value,
            username = user.UserName.Value,
            name = user.FirstName.Value,
            lastname = user.LastName.Value,
            email = user.Email.Value,
            avatar = user.Profile?.Avatar?.Url,
            bio = user.Profile?.Bio?.Value,
            location = user.Profile?.Location?.Value,
            website = user.Profile?.Website?.Url,
            verified = user.Profile.Verified,
            createdAt = user.CreatedAt?.Value.ToString(),
            followersCount = user.Profile.Followers.Value,
            followingCount = user.Profile.Following.Value,
            background = user.Profile?.Banner?.Url,
            pub = user.Pub?.Value
        };
    }
}