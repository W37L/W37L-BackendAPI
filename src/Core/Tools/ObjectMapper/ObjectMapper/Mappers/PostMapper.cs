using ObjectMapper.DTO;

namespace ObjectMapper.Mappers;

public class PostMapper : IMappingConfig<Post, ContentDTO> {
    public ContentDTO Map(Post content) {
        if (content == null) return null;
        ContentDTO c = new();
        c.PostId = content.Id.Value;
        c.UserId = content.Creator?.Id.Value;
        c.ParentPostId = content.ParentPost?.Id.Value;
        c.Type = content.PostType.ToString();
        c.Content = content.ContentTweet.Value;
        c.Likes = content.Likes.Value;
        c.CreatedAt = content.CreatedAt.Value.ToString();
        c.UserPub = content.Creator?.Pub.Value;
        c.Signature = content.Signature.Value;
        c.IsDeleted = false;
        return c;
    }
}