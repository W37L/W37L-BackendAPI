namespace W3TL.Core.Domain.Services;

public static class UnFollowService {
    public static Result Handle(User? follower, User? followee) {
        if (follower == null || followee == null)
            return Error.NullUser;

        if (!follower.Interactions.Following.Contains(followee.Id))
            return Result.Fail(Error.UserNotFollowed);

        try {
            //Update the follower
            follower.Interactions.Following.Remove(followee.Id);
            follower.Profile.Following.Decrement();

            //Update the followee
            followee.Interactions.Followers.Remove(follower.Id);
            followee.Profile.Followers.Decrement();

            return Result.Ok;
        }
        catch (Exception exception) {
            return Result.Fail(Error.FromException(exception));
        }
    }
}