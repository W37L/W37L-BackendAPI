using W3TL.Core.Domain.Agregates.User.Repository;

namespace UnitTests.Fakes;

public class InMemInteractionRepoStub : IInteractionRepository {
    private readonly Dictionary<string, HashSet<string>> _blocks = new();
    private readonly Dictionary<string, HashSet<string>> _follows = new();
    private readonly Dictionary<string, HashSet<string>> _highlights = new();

    // Implementing Likes, Retweets, etc. require a separate dictionary to handle the mapping between users and content.
    private readonly Dictionary<string, HashSet<string>> _likes = new();
    private readonly Dictionary<string, HashSet<string>> _mutes = new();
    private readonly Dictionary<string, HashSet<string>> _reports = new();
    private readonly Dictionary<string, HashSet<string>> _retweets = new();

    public Task<Result> BlockUserAsync(string userId, string blockedUserId) {
        if (!_blocks.ContainsKey(userId))
            _blocks[userId] = new HashSet<string>();

        _blocks[userId].Add(blockedUserId);
        return Task.FromResult(Result.Success());
    }

    public Task<Result> UnblockUserAsync(string userId, string blockedUserId) {
        if (_blocks.ContainsKey(userId) && _blocks[userId].Contains(blockedUserId)) {
            _blocks[userId].Remove(blockedUserId);
            return Task.FromResult(Result.Success());
        }

        return Task.FromResult(Result.Fail(Error.UserNotBlocked));
    }

    public Task<Result> FollowUserAsync(string userId, string followedUserId) {
        if (!_follows.ContainsKey(userId))
            _follows[userId] = new HashSet<string>();

        _follows[userId].Add(followedUserId);
        return Task.FromResult(Result.Success());
    }

    public Task<Result> UnfollowUserAsync(string userId, string followedUserId) {
        if (_follows.ContainsKey(userId) && _follows[userId].Contains(followedUserId)) {
            _follows[userId].Remove(followedUserId);
            return Task.FromResult(Result.Success());
        }

        return Task.FromResult(Result.Fail(Error.UserNotFollowed));
    }

    public Task<Result> MuteUserAsync(string userId, string mutedUserId) {
        if (!_mutes.ContainsKey(userId))
            _mutes[userId] = new HashSet<string>();

        _mutes[userId].Add(mutedUserId);
        return Task.FromResult(Result.Success());
    }

    public Task<Result> UnmuteUserAsync(string userId, string mutedUserId) {
        if (_mutes.ContainsKey(userId) && _mutes[userId].Contains(mutedUserId)) {
            _mutes[userId].Remove(mutedUserId);
            return Task.FromResult(Result.Success());
        }

        return Task.FromResult(Result.Fail(Error.UserNotMuted));
    }

    public Task<Result> ReportUserAsync(string userId, string reportedUserId) {
        if (!_reports.ContainsKey(userId))
            _reports[userId] = new HashSet<string>();

        _reports[userId].Add(reportedUserId);
        return Task.FromResult(Result.Success());
    }

    public Task<Result> UnreportUserAsync(string userId, string reportedUserId) {
        if (_reports.ContainsKey(userId) && _reports[userId].Contains(reportedUserId)) {
            _reports[userId].Remove(reportedUserId);
            return Task.FromResult(Result.Success());
        }

        return Task.FromResult(Result.Fail(Error.UserNotReported));
    }

    public Task<Result> LikeTweetAsync(string userId, string tweetId) {
        if (!_likes.ContainsKey(userId))
            _likes[userId] = new HashSet<string>();

        _likes[userId].Add(tweetId);
        return Task.FromResult(Result.Success());
    }

    public Task<Result> UnlikeTweetAsync(string userId, string tweetId) {
        if (_likes.ContainsKey(userId) && _likes[userId].Contains(tweetId)) {
            _likes[userId].Remove(tweetId);
            return Task.FromResult(Result.Success());
        }

        return Task.FromResult(Result.Fail(Error.ContentNotLiked));
    }

    public Task<Result> RetweetAsync(string userId, string tweetId) {
        if (!_retweets.ContainsKey(userId))
            _retweets[userId] = new HashSet<string>();

        _retweets[userId].Add(tweetId);
        return Task.FromResult(Result.Success());
    }

    public Task<Result> UnretweetAsync(string userId, string tweetId) {
        if (_retweets.ContainsKey(userId) && _retweets[userId].Contains(tweetId)) {
            _retweets[userId].Remove(tweetId);
            return Task.FromResult(Result.Success());
        }

        return Task.FromResult(Result.Fail(Error.ContentNotRetweeted));
    }

    public Task<Result> HighlightTweetAsync(string userId, string tweetId) {
        if (!_highlights.ContainsKey(userId))
            _highlights[userId] = new HashSet<string>();

        _highlights[userId].Add(tweetId);
        return Task.FromResult(Result.Success());
    }

    public Task<Result> UnhighlightTweetAsync(string userId, string tweetId) {
        if (_highlights.ContainsKey(userId) && _highlights[userId].Contains(tweetId)) {
            _highlights[userId].Remove(tweetId);
            return Task.FromResult(Result.Success());
        }

        return Task.FromResult(Result.Fail(Error.ContentNotHighlighted));
    }
}