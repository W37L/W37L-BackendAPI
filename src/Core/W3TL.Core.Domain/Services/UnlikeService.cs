using W3TL.Core.Domain.Agregates.Post;

namespace W3TL.Core.Domain.Services;

public class UnlikeService {
    public static Result Handle(User? unliker, Content? content) {
        var errors = new HashSet<Error>();

        if (unliker == null || content == null)
            return Error.NullUser;

        if (!unliker.Interactions.Likes.Contains(content.Id))
            errors.Add(Error.UserNotLiked);

        if (errors.Any())
            return Error.CompileErrors(errors);

        try {
            //Update the unliker
            unliker.Interactions.Likes.Remove(content.Id as PostId);

            //Update the post
            content.Likes.Decrement();

            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}