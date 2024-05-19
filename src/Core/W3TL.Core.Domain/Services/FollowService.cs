namespace W3TL.Core.Domain.Services;

/// <summary>
/// Provides methods for handling follow actions between users.
/// </summary>
public static class FollowService {
    
    /// <summary>
    /// Handles the follow action between a follower and a followee.
    /// </summary>
    /// <param name="follower">The user who wants to follow.</param>
    /// <param name="followee">The user who will be followed.</param>
    /// <returns>A result indicating the success or failure of the follow action.</returns>
    public static Result Handle(User? follower, User? followee) {
        HashSet<Error> errors = new HashSet<Error>();

        if (follower == null || followee == null)
            return Error.NullUser;
        if (follower == followee)
            errors.Add(Error.CannotFollowSelf);
        if (follower!.Interactions!.Blocked!.Contains(followee.Id))
            errors.Add(Error.UserBlocked);
        if (follower!.Interactions!.Following!.Contains(followee.Id))
            errors.Add(Error.UserAlreadyFollowed);

        if (errors.Any())
            return Error.CompileErrors(errors);

        try {
            //Update the follower
            follower.Interactions.Following.Add(followee.Id);
            follower.Profile.Following.Increment();

            //Update the followee
            followee.Interactions.Followers.Add(follower.Id);
            followee.Profile.Followers.Increment();

            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}