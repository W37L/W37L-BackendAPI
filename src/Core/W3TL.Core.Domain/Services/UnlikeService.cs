using W3TL.Core.Domain.Agregates.Post;

namespace W3TL.Core.Domain.Services;

public class UnlikeService {
    public static Result Handle(User? unliker, Content? content) {
        var errors = new HashSet<Error>();

        if (unliker == null || content == null)
            return Error.NullUser;

        if (!unliker.Likes.Contains(content))
            errors.Add(Error.UserNotLiked);

        if (errors.Any())
            return Error.CompileErrors(errors);

        try {
            //Update the unliker
            unliker.Likes.Remove(content);

            //Update the post
            content.Likes.Decrement();

            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}