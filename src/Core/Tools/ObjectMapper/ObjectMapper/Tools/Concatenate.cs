using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.User.Entity;

namespace ObjectMapper.Tools;

public static class Concatenate {
    public static Content Append(User user, Post post) {
        Reflexion.SetProperty(post, "Creator", user);
        return post;
    }

    public static Content Append(User user, Comment comment) {
        Reflexion.SetProperty(comment, "Creator", user);
        return comment;
    }

    public static Content Append(User user, Content content) {
        Reflexion.SetProperty(content, "Creator", user);
        return content;
    }

    public static Comment Append(Post post, Comment comment) {
        Reflexion.SetProperty(comment, "ParentPost", post);
        return comment;
    }

    public static User Appern(User user, Interactions interactions) {
        Reflexion.SetProperty(user, "Interactions", interactions);
        return user;
    }
}