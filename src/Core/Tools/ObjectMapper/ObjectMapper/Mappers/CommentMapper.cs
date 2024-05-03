using ObjectMapper.DTO;

namespace ObjectMapper.Mappers;

public class CommentMapper : IMappingConfig<Comment, ContentDTO> {
    public ContentDTO Map(Comment content) {
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
        c.IsDeleted = false; // Assuming a default or no corresponding domain model property
        if (content is Comment comm) {
            // c.MediaUrl = comm?.MediaUrl?.Url;
            // c.MediaType = comm?.MediaType.ToString();
            c.Thumbnail = null;
            c.Retweets = 0;
            c.Comments = 0; //TODO: Implement this
        }

        return c;
    }
}