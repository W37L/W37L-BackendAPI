namespace W3TL.Core.Domain.Services;

/// <summary>
/// Provides methods for handling unfollow actions between users.
/// </summary>
public static class UnFollowService {
    
    /// <summary>
    /// Handles the unfollow action between a follower and a followee.
    /// </summary>
    /// <param name="follower">The user who wants to unfollow.</param>
    /// <param name="followee">The user who will be unfollowed.</param>
    /// <returns>A result indicating the success or failure of the unfollow action.</returns>
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