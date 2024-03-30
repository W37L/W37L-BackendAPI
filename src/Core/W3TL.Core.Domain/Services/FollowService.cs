namespace W3TL.Core.Domain.Services;

public static class FollowService {
    public static Result Handle(User? follower, User? followee) {
        HashSet<Error> errors = new HashSet<Error>();

        if (follower == null || followee == null)
            return Error.NullUser;
        if (follower == followee)
            errors.Add(Error.CannotFollowSelf);
        if (follower.Blocked.Contains(followee) || followee.Blocked.Contains(follower))
            errors.Add(Error.UserBlocked);
        if (follower.Following.Contains(followee))
            errors.Add(Error.UserAlreadyFollowed);

        if (errors.Any())
            return Error.CompileErrors(errors);

        try {
            follower.Following.Add(followee);
            followee.Followers.Add(follower);
            return Result.Ok;
        } catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}
