using System.Reflection;
using Persistence.UserPersistence.Firebase;
using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.User.Entity;
using W3TL.Core.Domain.Agregates.User.Entity.Values;
using W3TL.Core.Domain.Agregates.User.Values;
using W3TL.Core.Domain.Common.Values;

namespace Persistence.UserPersistence;

public class Mapper {
    public static Result<FirebaseUser> MapDomainUserToFirebaseUser(User user) {
        var firebaseUser = new FirebaseUser {
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

        return firebaseUser;
    }

    private static Result<FirebaseInteractions> MapDomainInteractionToFirebaseInteraction(Interactions interactions) {
        var firebaseInteractions = new FirebaseInteractions {
            blockedUsers = interactions.Blocked?.Select(b => b.Value).ToList(),
            followers = interactions.Followers?.Select(f => f.Value).ToList(),
            following = interactions.Following?.Select(f => f.Value).ToList(),
            highlightedTweetIds = interactions.Highlights?.Select(h => h.Value).ToList(),
            likedTweetIds = interactions.Likes?.Select(l => l.Value).ToList(),
            mutedUsers = interactions.Muted?.Select(m => m.Value).ToList(),
            reportedUsers = interactions.ReportedUsers?.Select(r => r.Value).ToList(),
            retweetedTweetIds = interactions.RetweetedTweets?.Select(rt => rt.Value).ToList()
        };
        return firebaseInteractions;
    }


    public static Result<User> MapFirebaseUserToDomainUser(FirebaseUser firebaseUser) {
        // Use reflection to access the private constructor
        var privateConstructor = typeof(User).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            new Type[] { }, // Match the parameters of the private constructor
            null);

        // Invoke the constructor to create a new instance of User
        var user = (User)privateConstructor.Invoke(new object[] { });

        // Create the object using the public constructor

        var userId = UserID.Create(firebaseUser.userId)
            .OnFailure(error => throw new InvalidOperationException(error.Message));
        var userName = UserNameType.Create(firebaseUser.username)
            .OnFailure(error => throw new InvalidOperationException(error.Message));
        var firstName = NameType.Create(firebaseUser.name)
            .OnFailure(error => throw new InvalidOperationException(error.Message));
        var lastName = LastNameType.Create(firebaseUser.lastname)
            .OnFailure(error => throw new InvalidOperationException(error.Message));
        var email = EmailType.Create(firebaseUser.email)
            .OnFailure(error => throw new InvalidOperationException(error.Message));
        var pub = PubType.Create(firebaseUser.pub)
            .OnFailure(error => throw new InvalidOperationException(error.Message));
        var createdAt = CreatedAtType.Create(firebaseUser.createdAt)
            .OnFailure(error => throw new InvalidOperationException(error.Message));


        // Set the properties using reflection
        SetProperty(user, "Id", userId.Payload);
        SetProperty(user, "UserName", userName.Payload);
        SetProperty(user, "FirstName", firstName.Payload);
        SetProperty(user, "LastName", lastName.Payload);
        SetProperty(user, "Email", email.Payload);
        SetProperty(user, "Pub", pub.Payload);
        SetProperty(user, "CreatedAt", createdAt.Payload);
        // and so on for other properties...

        var privateProfileConstructor = typeof(Profile).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            new Type[] { }, // Match the parameters of the private constructor
            null);

        var profile = (Profile)privateProfileConstructor.Invoke(new object[] { });

        if (!string.IsNullOrWhiteSpace(firebaseUser.avatar)) {
            var avatar = AvatarType.Create(firebaseUser.avatar)
                .OnFailure(error => throw new InvalidOperationException(error.Message));
            if (avatar.IsSuccess)
                SetProperty(profile, "Avatar", avatar.Payload);
        }

        if (!string.IsNullOrWhiteSpace(firebaseUser.background)) {
            var banner = BannerType.Create(firebaseUser.background)
                .OnFailure(error => throw new InvalidOperationException(error.Message));
            if (banner.IsSuccess)
                SetProperty(profile, "Banner", banner.Payload);
        }

        if (!string.IsNullOrWhiteSpace(firebaseUser.bio)) {
            var bio = BioType.Create(firebaseUser.bio)
                .OnFailure(error => throw new InvalidOperationException(error.Message));
            if (bio.IsSuccess)
                SetProperty(profile, "Bio", bio.Payload);
        }

        if (!string.IsNullOrWhiteSpace(firebaseUser.location)) {
            var location = LocationType.Create(firebaseUser.location)
                .OnFailure(error => throw new InvalidOperationException(error.Message));
            if (location.IsSuccess)
                SetProperty(profile, "Location", location.Payload);
        }

        if (!string.IsNullOrWhiteSpace(firebaseUser.website)) {
            var website = WebsiteType.Create(firebaseUser.website)
                .OnFailure(error => throw new InvalidOperationException(error.Message));
            if (website.IsSuccess)
                SetProperty(profile, "Website", website.Payload);
        }

        var followers = Count.Create(firebaseUser.followersCount)
            .OnFailure(error => throw new InvalidOperationException(error.Message));
        if (followers.IsSuccess)
            SetProperty(profile, "Followers", followers.Payload);

        var following = Count.Create(firebaseUser.followingCount)
            .OnFailure(error => throw new InvalidOperationException(error.Message));
        if (following.IsSuccess)
            SetProperty(profile, "Following", following.Payload);

        // ser the profile to the user
        SetProperty(user, "Profile", profile);

        // Map interactions
        SetProperty(user, "Interactions",
            MapFirebaseInteractionsToDomainInteractions(firebaseUser.Interactions).Payload);

        return user;
    }

    public static Result<Interactions> MapFirebaseInteractionsToDomainInteractions(
        FirebaseInteractions firebaseInteractions) {
        var privateConstructor = typeof(Interactions).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            new Type[] { }, // Match the parameters of the private constructor
            null);

        var interactions = (Interactions)privateConstructor.Invoke(new object[] { });

        SetProperty(interactions, "Blocked",
            firebaseInteractions?.blockedUsers == null
                ? new List<UserID>()
                : firebaseInteractions?.blockedUsers?.Select(b => UserID.Create(b).Payload).ToList());
        SetProperty(interactions, "Followers",
            firebaseInteractions?.followers == null
                ? new List<UserID>()
                : firebaseInteractions?.followers?.Select(f => UserID.Create(f).Payload).ToList());
        SetProperty(interactions, "Following",
            firebaseInteractions?.following == null
                ? new List<UserID>()
                : firebaseInteractions?.following?.Select(f => UserID.Create(f).Payload).ToList());
        SetProperty(interactions, "Highlights",
            firebaseInteractions?.highlightedTweetIds == null
                ? new List<PostId>()
                : firebaseInteractions?.highlightedTweetIds?.Select(h => PostId.Create(h).Payload).ToList());
        SetProperty(interactions, "Likes",
            firebaseInteractions?.likedTweetIds == null
                ? new List<PostId>()
                : firebaseInteractions?.likedTweetIds?.Select(l => PostId.Create(l).Payload).ToList());
        SetProperty(interactions, "Muted",
            firebaseInteractions?.mutedUsers == null
                ? new List<UserID>()
                : firebaseInteractions?.mutedUsers?.Select(m => UserID.Create(m).Payload).ToList());
        SetProperty(interactions, "ReportedUsers",
            firebaseInteractions?.reportedUsers == null
                ? new List<UserID>()
                : firebaseInteractions?.reportedUsers?.Select(r => UserID.Create(r).Payload).ToList());
        SetProperty(interactions, "RetweetedTweets",
            firebaseInteractions?.retweetedTweetIds == null
                ? new List<PostId>()
                : firebaseInteractions?.retweetedTweetIds?.Select(rt => PostId.Create(rt).Payload).ToList());
        return interactions;
    }


    private static void SetProperty(object obj, string propertyName, object value) {
        var propertyInfo = obj.GetType().GetProperty(
            propertyName,
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
        );

        // Ensure the property exists and has a setter
        if (propertyInfo != null && propertyInfo.CanWrite) {
            // If the property has a private setter, it still needs to be accessed explicitly
            var setMethod = propertyInfo.GetSetMethod(true);
            if (setMethod != null)
                setMethod.Invoke(obj, new[] { value });
            else
                throw new InvalidOperationException($"Property '{propertyName}' does not have a setter.");
        }
        else {
            throw new InvalidOperationException($"Property '{propertyName}' not found on type '{obj.GetType()}'.");
        }
    }
}