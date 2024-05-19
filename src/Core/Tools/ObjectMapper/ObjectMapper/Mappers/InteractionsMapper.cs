using Persistence.UserPersistence.Firebase;
using W3TL.Core.Domain.Agregates.User.Entity;

namespace ObjectMapper.Mappers;

/// <summary>
///     The InteractionsMapper class is responsible for mapping Interactions objects to InteractionsDTO objects.
/// </summary>
public class InteractionsMapper : IMappingConfig<Interactions, InteractionsDTO> {
    /// <summary>
    ///     Maps an instance of Interactions to an instance of InteractionsDTO.
    /// </summary>
    /// <param name="interactions">The Interactions instance to be mapped.</param>
    /// <returns>The mapped InteractionsDTO instance.</returns>
    public InteractionsDTO Map(Interactions interactions) {
        return new InteractionsDTO {
            blockedUsers = interactions.Blocked?.Select(b => b.Value).ToList(),
            followers = interactions.Followers?.Select(f => f.Value).ToList(),
            following = interactions.Following?.Select(f => f.Value).ToList(),
            highlightedTweetIds = interactions.Highlights?.Select(h => h.Value).ToList(),
            likedTweetIds = interactions.Likes?.Select(l => l.Value).ToList(),
            mutedUsers = interactions.Muted?.Select(m => m.Value).ToList(),
            reportedUsers = interactions.ReportedUsers?.Select(r => r.Value).ToList(),
            retweetedTweetIds = interactions.RetweetedTweets?.Select(rt => rt.Value).ToList()
        };
    }
}