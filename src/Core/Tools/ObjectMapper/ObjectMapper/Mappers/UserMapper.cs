using Persistence.UserPersistence;

namespace ObjectMapper.Mappers;

public class UserMapper : IMappingConfig<User, UserDTO> {
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