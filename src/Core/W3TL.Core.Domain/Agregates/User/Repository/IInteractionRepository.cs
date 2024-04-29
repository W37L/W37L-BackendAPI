namespace W3TL.Core.Domain.Agregates.User.Repository;

public interface IInteractionRepository {
    Task<Result> BlockUserAsync(string userId, string blockedUserId);
    Task<Result> UnblockUserAsync(string userId, string blockedUserId);
    Task<Result> FollowUserAsync(string userId, string followedUserId);
    Task<Result> UnfollowUserAsync(string userId, string followedUserId);
    Task<Result> MuteUserAsync(string userId, string mutedUserId);
    Task<Result> UnmuteUserAsync(string userId, string mutedUserId);
    Task<Result> ReportUserAsync(string userId, string reportedUserId);
    Task<Result> UnreportUserAsync(string userId, string reportedUserId);
    Task<Result> LikeTweetAsync(string userId, string tweetId);
    Task<Result> UnlikeTweetAsync(string userId, string tweetId);
    Task<Result> RetweetAsync(string userId, string tweetId);
    Task<Result> UnretweetAsync(string userId, string tweetId);
    Task<Result> HighlightTweetAsync(string userId, string tweetId);
    Task<Result> UnhighlightTweetAsync(string userId, string tweetId);
}