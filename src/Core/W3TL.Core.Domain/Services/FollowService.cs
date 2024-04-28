namespace W3TL.Core.Domain.Services;

public static class FollowService {
    public static Result Handle(User? follower, User? followee) {
        HashSet<Error> errors = new HashSet<Error>();

        if (follower == null || followee == null)
            return Error.NullUser;
        if (follower == followee)
            errors.Add(Error.CannotFollowSelf);
        if (follower!.Interactions!.Blocked!.Contains(followee.Id))
            errors.Add(Error.UserBlocked);
        if (follower.Interactions!.Following!.Contains(followee.Id))
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