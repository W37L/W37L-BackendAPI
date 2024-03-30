namespace W3TL.Core.Domain.Services;

public static class UnFollowService {
    public static Result Handle(User? follower, User? followee) {
        if (follower == null || followee == null)
            return Error.NullUser;

        if (!follower.Following.Contains(followee))
            return Result.Fail(Error.UserNotFollowed);

        try {
            follower.Following.Remove(followee);
            followee.Followers.Remove(follower);
            return Result.Ok;
        } catch (Exception exception) {
            return Result.Fail(Error.FromException(exception));
        }
    }
}