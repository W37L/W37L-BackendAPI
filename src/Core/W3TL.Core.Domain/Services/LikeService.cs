using W3TL.Core.Domain.Agregates.Post;

namespace W3TL.Core.Domain.Services;

public class LikeService {
    public static Result Handle(User? liker, Content? content) {
        var errors = new HashSet<Error>();

        if (liker == null || content == null)
            return Error.NullUser;

        if (liker.Likes.Contains(content))
            errors.Add(Error.UserAlreadyLiked);

        if (content.Creator.Blocked.Contains(liker) || liker.Blocked.Contains(content.Creator))
            errors.Add(Error.UserBlocked);

        if (errors.Any())
            return Error.CompileErrors(errors);

        try {
            //Update the liker
            liker.Likes.Add(content);

            //Update the post
            content.Likes.Increment();

            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}