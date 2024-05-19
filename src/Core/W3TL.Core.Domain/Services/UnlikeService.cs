using W3TL.Core.Domain.Agregates.Post;

namespace W3TL.Core.Domain.Services;

/// <summary>
/// Provides methods for handling unlike actions on content by users.
/// </summary>
public class UnlikeService {
    
    /// <summary>
    /// Handles the unlike action on content by a user.
    /// </summary>
    /// <param name="unliker">The user who wants to unlike the content.</param>
    /// <param name="content">The content to be unliked.</param>
    /// <returns>A result indicating the success or failure of the unlike action.</returns>
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