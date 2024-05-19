using System.Reflection;
using ObjectMapper.Tools;
using Persistence.UserPersistence;
using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.User.Entity;
using W3TL.Core.Domain.Agregates.User.Entity.Values;
using W3TL.Core.Domain.Agregates.User.Values;
using W3TL.Core.Domain.Common.Values;

namespace ObjectMapper.Mappers;

/// <summary>
///     UserDomainMapper class is responsible for mapping between UserDTO and User objects.
/// </summary>
public class UserDomainMapper : IMappingConfig<UserDTO, User> {
    
    /// <summary>
    ///     Maps a UserDTO object to a User object.
    /// </summary>
    /// <param name="input">The UserDTO object to be mapped.</param>
    /// <returns>A User object.</returns>
    public User Map(UserDTO input) {
        // Use reflection to access the private constructor
        var privateConstructor = typeof(User).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            new Type[] { }, // Match the parameters of the private constructor
            null);

        // Invoke the constructor to create a new instance of User
        var user = (User)privateConstructor.Invoke(new object[] { });

        // Create the object using the public constructor

        var userId = UserID.Create(input.userId)
            .OnFailure(error => throw new InvalidOperationException(error.Message));
        var userName = UserNameType.Create(input.username)
            .OnFailure(error => throw new InvalidOperationException(error.Message));
        var firstName = NameType.Create(input.name)
            .OnFailure(error => throw new InvalidOperationException(error.Message));
        var lastName = LastNameType.Create(input.lastname)
            .OnFailure(error => throw new InvalidOperationException(error.Message));
        var email = EmailType.Create(input.email)
            .OnFailure(error => throw new InvalidOperationException(error.Message));
        var pub = PubType.Create(input.pub)
            .OnFailure(error => throw new InvalidOperationException(error.Message));
        var createdAt = CreatedAtType.Create(input.createdAt)
            .OnFailure(error => throw new InvalidOperationException(error.Message));


        // Set the properties using reflection
        Reflexion.SetProperty(user, "Id", userId.Payload);
        Reflexion.SetProperty(user, "UserName", userName.Payload);
        Reflexion.SetProperty(user, "FirstName", firstName.Payload);
        Reflexion.SetProperty(user, "LastName", lastName.Payload);
        Reflexion.SetProperty(user, "Email", email.Payload);
        Reflexion.SetProperty(user, "Pub", pub.Payload);
        Reflexion.SetProperty(user, "CreatedAt", createdAt.Payload);
        // and so on for other properties...

        var privateProfileConstructor = typeof(Profile).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            new Type[] { }, // Match the parameters of the private constructor
            null);

        var profile = (Profile)privateProfileConstructor.Invoke(new object[] { });

        if (!string.IsNullOrWhiteSpace(input.avatar)) {
            var avatar = AvatarType.Create(input.avatar)
                .OnFailure(error => throw new InvalidOperationException(error.Message));
            if (avatar.IsSuccess)
                Reflexion.SetProperty(profile, "Avatar", avatar.Payload);
        }

        if (!string.IsNullOrWhiteSpace(input.background)) {
            var banner = BannerType.Create(input.background)
                .OnFailure(error => throw new InvalidOperationException(error.Message));
            if (banner.IsSuccess)
                Reflexion.SetProperty(profile, "Banner", banner.Payload);
        }

        if (!string.IsNullOrWhiteSpace(input.bio)) {
            var bio = BioType.Create(input.bio)
                .OnFailure(error => throw new InvalidOperationException(error.Message));
            if (bio.IsSuccess)
                Reflexion.SetProperty(profile, "Bio", bio.Payload);
        }

        if (!string.IsNullOrWhiteSpace(input.location)) {
            var location = LocationType.Create(input.location)
                .OnFailure(error => throw new InvalidOperationException(error.Message));
            if (location.IsSuccess)
                Reflexion.SetProperty(profile, "Location", location.Payload);
        }

        if (!string.IsNullOrWhiteSpace(input.website)) {
            var website = WebsiteType.Create(input.website)
                .OnFailure(error => throw new InvalidOperationException(error.Message));
            if (website.IsSuccess)
                Reflexion.SetProperty(profile, "Website", website.Payload);
        }

        var followers = Count.Create(input.followersCount)
            .OnFailure(error => throw new InvalidOperationException(error.Message));
        if (followers.IsSuccess)
            Reflexion.SetProperty(profile, "Followers", followers.Payload);

        var following = Count.Create(input.followingCount)
            .OnFailure(error => throw new InvalidOperationException(error.Message));
        if (following.IsSuccess)
            Reflexion.SetProperty(profile, "Following", following.Payload);

        // ser the profile to the user
        Reflexion.SetProperty(user, "Profile", profile);

        //TODO: Add the rest of the properties
        // // Map interactions
        if (input.Interactions != null) {
            InteractionsDomainMapper interactionsMapper = new InteractionsDomainMapper();
            var interactions = interactionsMapper.Map(input.Interactions);
            Reflexion.SetProperty(user, "Interactions", interactions);
        }


        return user;
    }
}