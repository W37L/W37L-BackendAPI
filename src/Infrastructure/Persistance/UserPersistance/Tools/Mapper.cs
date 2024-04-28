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
        var firebaseUser = new FirebaseUser();
        firebaseUser.userId = user.Id.Value;
        firebaseUser.username = user.UserName.Value;
        firebaseUser.name = user.FirstName.Value;
        firebaseUser.lastname = user.LastName.Value;
        firebaseUser.email = user.Email.Value;
        firebaseUser.avatar = user.Profile?.Avatar?.Url;
        firebaseUser.bio = user.Profile?.Bio?.Value;
        firebaseUser.location = user.Profile?.Location?.Value;
        firebaseUser.website = user.Profile?.Website?.Url;
        firebaseUser.verified = user.Profile.Verified;
        firebaseUser.createdAt = user.CreatedAt?.Value.ToString();
        firebaseUser.followersCount = user.Profile.Followers.Value;
        firebaseUser.followingCount = user.Profile.Following.Value;
        firebaseUser.background = user.Profile?.Banner?.Url;
        return firebaseUser;
    }

    public static Result<FirebaseInteractions> MapDomainInteractionToFirebaseInteraction(Interactions interactions) {
        var FirebaseInteractions = new FirebaseInteractions();
        FirebaseInteractions.blockedUsers = interactions.Blocked?.Select(b => b.Value).ToList();
        FirebaseInteractions.followers = interactions.Followers?.Select(f => f.Value).ToList();
        FirebaseInteractions.following = interactions.Following?.Select(f => f.Value).ToList();
        FirebaseInteractions.highlightedTweetIds = interactions.Highlights?.Select(h => h.Value).ToList();
        FirebaseInteractions.likedTweetIds = interactions.Likes?.Select(l => l.Value).ToList();
        FirebaseInteractions.mutedUsers = interactions.Muted?.Select(m => m.Value).ToList();
        FirebaseInteractions.reportedUsers = interactions.ReportedUsers?.Select(r => r.Value).ToList();
        FirebaseInteractions.retweetedTweetIds = interactions.RetweetedTweets?.Select(r => r.Value).ToList();
        return FirebaseInteractions;
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
            firebaseInteractions.blockedUsers.Select(b => UserID.Create(b).Payload).ToList());
        SetProperty(interactions, "Followers",
            firebaseInteractions.followers.Select(f => UserID.Create(f).Payload).ToList());
        SetProperty(interactions, "Following",
            firebaseInteractions.following.Select(f => UserID.Create(f).Payload).ToList());
        SetProperty(interactions, "Highlights",
            firebaseInteractions.highlightedTweetIds.Select(h => PostId.Create(h).Payload).ToList());
        SetProperty(interactions, "Likes",
            firebaseInteractions.likedTweetIds.Select(l => PostId.Create(l).Payload).ToList());
        SetProperty(interactions, "Muted",
            firebaseInteractions.mutedUsers.Select(m => UserID.Create(m).Payload).ToList());
        SetProperty(interactions, "ReportedUsers",
            firebaseInteractions.reportedUsers.Select(r => UserID.Create(r).Payload).ToList());
        SetProperty(interactions, "RetweetedTweets",
            firebaseInteractions.retweetedTweetIds.Select(r => PostId.Create(r).Payload).ToList());
        return interactions;
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
        // var pub = PubType.Create(firebaseUser.pub).OnFailure(error => throw new InvalidOperationException(error.Message));
        var createdAt = CreatedAtType.Create(firebaseUser.createdAt)
            .OnFailure(error => throw new InvalidOperationException(error.Message));


        // Set the properties using reflection
        SetProperty(user, "Id", userId.Payload);
        SetProperty(user, "UserName", userName.Payload);
        SetProperty(user, "FirstName", firstName.Payload);
        SetProperty(user, "LastName", lastName.Payload);
        SetProperty(user, "Email", email.Payload);
        // SetProperty(user, "Pub", pub.Payload);
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

        // //map the lists
        // SetProperty(user, "Followers", firebaseUser.followers.Select(f => UserID.Create(f).Payload).ToList());
        // SetProperty(user, "Following", firebaseUser.following.Select(f => UserID.Create(f).Payload).ToList());
        // SetProperty(user, "Blocked", firebaseUser.blockedUsers.Select(f => UserID.Create(f).Payload).ToList());
        // SetProperty(user, "Muted", firebaseUser.mutedUsers.Select(f => UserID.Create(f).Payload).ToList());
        // SetProperty(user, "Likes", firebaseUser.likedTweetIds.Select(f => PostId.Create(f).Payload).ToList());
        // SetProperty(user, "Posts", firebaseUser.highlightedTweetIds.Select(f => PostId.Create(f).Payload).ToList());
        // SetProperty(user, "Highlights", firebaseUser.highlightedTweetIds.Select(f => PostId.Create(f).Payload).ToList());
        // SetProperty(user, "ReportedUsers", firebaseUser.reportedUsers.Select(f => UserID.Create(f).Payload).ToList());
        // SetProperty(user, "RetweetedTweets", firebaseUser.retweetedTweetIds.Select(f => PostId.Create(f).Payload).ToList());
        //

        return user;
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