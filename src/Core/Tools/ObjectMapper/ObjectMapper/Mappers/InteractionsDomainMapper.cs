using System.Reflection;
using ObjectMapper.Tools;
using Persistence.UserPersistence.Firebase;
using W3TL.Core.Domain.Agregates.User.Entity;
using W3TL.Core.Domain.Common.Values;

namespace ObjectMapper.Mappers;

/// <summary>
///     Class responsible for mapping InteractionsDTO objects to Interactions objects.
/// </summary>
public class InteractionsDomainMapper : IMappingConfig<InteractionsDTO, Interactions> {
    
    /// <summary>
    ///     Maps an instance of InteractionsDTO to an instance of Interactions.
    /// </summary>
    /// <param name="input">The InteractionsDTO instance to be mapped.</param>
    /// <returns>The mapped Interactions instance.</returns>
    public Interactions Map(InteractionsDTO input) {
        var privateConstructor = typeof(Interactions).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            new Type[] { }, // Match the parameters of the private constructor
            null);

        var interactions = (Interactions)privateConstructor.Invoke(new object[] { });

        Reflexion.SetProperty(interactions, "Blocked",
            input?.blockedUsers == null
                ? new List<UserID>()
                : input?.blockedUsers?.Select(b => UserID.Create(b).Payload).ToList());
        Reflexion.SetProperty(interactions, "Followers",
            input?.followers == null
                ? new List<UserID>()
                : input?.followers?.Select(f => UserID.Create(f).Payload).ToList());
        Reflexion.SetProperty(interactions, "Following",
            input?.following == null
                ? new List<UserID>()
                : input?.following?.Select(f => UserID.Create(f).Payload).ToList());
        Reflexion.SetProperty(interactions, "Highlights",
            input?.highlightedTweetIds == null
                ? new List<PostId>()
                : input?.highlightedTweetIds?.Select(h => PostId.Create(h).Payload).ToList());
        Reflexion.SetProperty(interactions, "Likes",
            input?.likedTweetIds == null
                ? new List<PostId>()
                : input?.likedTweetIds?.Select(l => PostId.Create(l).Payload).ToList());
        Reflexion.SetProperty(interactions, "Muted",
            input?.mutedUsers == null
                ? new List<UserID>()
                : input?.mutedUsers?.Select(m => UserID.Create(m).Payload).ToList());
        Reflexion.SetProperty(interactions, "ReportedUsers",
            input?.reportedUsers == null
                ? new List<UserID>()
                : input?.reportedUsers?.Select(r => UserID.Create(r).Payload).ToList());
        Reflexion.SetProperty(interactions, "RetweetedTweets",
            input?.retweetedTweetIds == null
                ? new List<PostId>()
                : input?.retweetedTweetIds?.Select(rt => PostId.Create(rt).Payload).ToList());
        return interactions;
    }
}