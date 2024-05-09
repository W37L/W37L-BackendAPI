using W3TL.Core.Domain.Agregates.User.Repository;

namespace UnitTests.Fakes;

public class InMemInteractionRepoStub : IInteractionRepository {
    public Task<Result> BlockUserAsync(string userId, string blockedUserId) {
        throw new NotImplementedException();
    }

    public Task<Result> UnblockUserAsync(string userId, string blockedUserId) {
        throw new NotImplementedException();
    }

    public Task<Result> FollowUserAsync(string userId, string followedUserId) {
        throw new NotImplementedException();
    }

    public Task<Result> UnfollowUserAsync(string userId, string followedUserId) {
        throw new NotImplementedException();
    }

    public Task<Result> MuteUserAsync(string userId, string mutedUserId) {
        throw new NotImplementedException();
    }

    public Task<Result> UnmuteUserAsync(string userId, string mutedUserId) {
        throw new NotImplementedException();
    }

    public Task<Result> ReportUserAsync(string userId, string reportedUserId) {
        throw new NotImplementedException();
    }

    public Task<Result> UnreportUserAsync(string userId, string reportedUserId) {
        throw new NotImplementedException();
    }

    public Task<Result> LikeTweetAsync(string userId, string tweetId) {
        throw new NotImplementedException();
    }

    public Task<Result> UnlikeTweetAsync(string userId, string tweetId) {
        throw new NotImplementedException();
    }

    public Task<Result> RetweetAsync(string userId, string tweetId) {
        throw new NotImplementedException();
    }

    public Task<Result> UnretweetAsync(string userId, string tweetId) {
        throw new NotImplementedException();
    }

    public Task<Result> HighlightTweetAsync(string userId, string tweetId) {
        throw new NotImplementedException();
    }

    public Task<Result> UnhighlightTweetAsync(string userId, string tweetId) {
        throw new NotImplementedException();
    }
}