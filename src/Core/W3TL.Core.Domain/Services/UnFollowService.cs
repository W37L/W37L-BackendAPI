namespace W3TL.Core.Domain.Services;

public static class UnFollowService {
    public static Result Handle(User? follower, User? followee) {
        if (follower == null || followee == null)
            return Error.NullUser;

        if (!follower.Following.Contains(followee))
            return Result.Fail(Error.UserNotFollowed);

        try {
            //Update the follower
            follower.Following.Remove(followee);
            follower.Profile.Following.Decrement();

            //Update the followee
            followee.Followers.Remove(follower);
            followee.Profile.Followers.Decrement();

            return Result.Ok;
        }
        catch (Exception exception) {
            return Result.Fail(Error.FromException(exception));
        }
    }
}