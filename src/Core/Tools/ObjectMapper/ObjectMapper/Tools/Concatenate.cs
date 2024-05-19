using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.User.Entity;

namespace ObjectMapper.Tools;

// Utility class for concatenating domain entities
public static class Concatenate {
    /// <summary>
    ///     Appends a User as the Creator to a Post, Comment, or Content object.
    /// </summary>
    /// <param name="user">The user to set as creator.</param>
    /// <param name="content">The content object to append to.</param>
    /// <returns>The updated content object with the user set as creator.</returns>
    public static Content Append(User user, Post post) {
        Reflexion.SetProperty(post, "Creator", user);
        return post;
    }

    /// <summary>
    ///     Appends a User as the Creator to a Post, Comment, or Content object.
    /// </summary>
    /// <param name="user">The user to set as creator.</param>
    /// <param name="comment">The comment object to append to.</param>
    /// <returns>The updated comment object with the user set as creator.</returns>
    public static Content Append(User user, Comment comment) {
        Reflexion.SetProperty(comment, "Creator", user);
        return comment;
    }

    /// <summary>
    ///     Appends a User as the Creator to a Post, Comment, or Content object.
    /// </summary>
    /// <param name="user">The user to set as creator.</param>
    /// <param name="content">The content object to append to.</param>
    /// <returns>The updated content object with the user set as creator.</returns>
    public static Content Append(User user, Content content) {
        Reflexion.SetProperty(content, "Creator", user);
        return content;
    }

    /// <summary>
    ///     Appends a User as the Creator to a Post, Comment, or Content object.
    /// </summary>
    /// <param name="user">The user to set as creator.</param>
    /// <param name="content">The content object to append to.</param>
    /// <returns>The updated content object with the user set as creator.</returns>
    public static Comment Append(Post post, Comment comment) {
        Reflexion.SetProperty(comment, "ParentPost", post);
        return comment;
    }

    /// <summary>
    ///     Appends an Interactions object to a User.
    /// </summary>
    /// <param name="user">The User object to append the Interactions object to.</param>
    /// <param name="interactions">The Interactions object to append.</param>
    /// <returns>The User object with the Interactions object appended.</returns>
    public static User Appern(User user, Interactions interactions) {
        Reflexion.SetProperty(user, "Interactions", interactions);
        return user;
    }
}