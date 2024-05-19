using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.User.Entity;

namespace W3TL.Core.Domain.Services;

/// <summary>
/// Provides methods for handling like actions on content by users.
/// </summary>
public class LikeService {
    
    /// <summary>
    /// Handles the like action on content by a user.
    /// </summary>
    /// <param name="liker">The user who wants to like the content.</param>
    /// <param name="content">The content to be liked.</param>
    /// <returns>A result indicating the success or failure of the like action.</returns>
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