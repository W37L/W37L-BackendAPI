namespace W3TL.Core.Domain.Agregates.User.Repository;

/// <summary>
/// Represents a repository for user interactions.
/// </summary>
public interface IInteractionRepository
{
    /// <summary>
    /// Blocks a user identified by their ID.
    /// </summary>
    Task<Result> BlockUserAsync(string userId, string blockedUserId);

    /// <summary>
    /// Unblocks a previously blocked user identified by their ID.
    /// </summary>
    Task<Result> UnblockUserAsync(string userId, string blockedUserId);

    /// <summary>
    /// Follows a user identified by their ID.
    /// </summary>
    Task<Result> FollowUserAsync(string userId, string followedUserId);

    /// <summary>
    /// Unfollows a user identified by their ID.
    /// </summary>
    Task<Result> UnfollowUserAsync(string userId, string followedUserId);

    /// <summary>
    /// Mutes a user identified by their ID.
    /// </summary>
    Task<Result> MuteUserAsync(string userId, string mutedUserId);

    /// <summary>
    /// Unmutes a previously muted user identified by their ID.
    /// </summary>
    Task<Result> UnmuteUserAsync(string userId, string mutedUserId);

    /// <summary>
    /// Reports a user identified by their ID.
    /// </summary>
    Task<Result> ReportUserAsync(string userId, string reportedUserId);

    /// <summary>
    /// Removes a report for a user identified by their ID.
    /// </summary>
    Task<Result> UnreportUserAsync(string userId, string reportedUserId);

    /// <summary>
    /// Likes a tweet identified by its ID.
    /// </summary>
    Task<Result> LikeTweetAsync(string userId, string tweetId);

    /// <summary>
    /// Removes a like from a tweet identified by its ID.
    /// </summary>
    Task<Result> UnlikeTweetAsync(string userId, string tweetId);

    /// <summary>
    /// Retweets a tweet identified by its ID.
    /// </summary>
    Task<Result> RetweetAsync(string userId, string tweetId);

    /// <summary>
    /// Removes a retweet from a tweet identified by its ID.
    /// </summary>
    Task<Result> UnretweetAsync(string userId, string tweetId);

    /// <summary>
    /// Highlights a tweet identified by its ID.
    /// </summary>
    Task<Result> HighlightTweetAsync(string userId, string tweetId);

    /// <summary>
    /// Removes a highlight from a tweet identified by its ID.
    /// </summary>
    Task<Result> UnhighlightTweetAsync(string userId, string tweetId);
}
