using W3TL.Core.Domain.Agregates.Post;

namespace ObjectMapper.Tools;

public static class Concatenate {
    public static Content Append(User user, Content content) {
        Reflexion.SetProperty(content, "Creator", user);
        return content;
    }
}