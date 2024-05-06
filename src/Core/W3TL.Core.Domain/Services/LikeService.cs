using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.User.Entity;

namespace W3TL.Core.Domain.Services;

public class LikeService {
    public static Result Handle(User? liker, Content? content) {
        var errors = new HashSet<Error>();

        if (liker == null || content == null)
            return Error.NullUser;

        if (liker.Interactions == null)
            liker.Interactions = Interactions.Create(liker.Id).Payload;

        if (liker.Interactions.Likes.Contains(content.Id))
            errors.Add(Error.UserAlreadyLiked);

        if (content.Creator.Interactions.Blocked.Contains(liker.Id) ||
            liker.Interactions.Blocked.Contains(content.Creator.Id))
            errors.Add(Error.UserBlocked);

        if (errors.Any())
            return Error.CompileErrors(errors);

        try {
            //Update the liker
            liker.Interactions.Likes.Add(content.Id as PostId);

            //Update the post
            content.Likes.Increment();

            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}