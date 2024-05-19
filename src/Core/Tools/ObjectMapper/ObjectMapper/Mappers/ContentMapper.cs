using ObjectMapper.DTO;
using W3TL.Core.Domain.Agregates.Post;

namespace ObjectMapper.Mappers;

public class ContentMapper : IMappingConfig<Content, ContentDTO> {
    /// <summary>
    ///     Maps a <see cref="Content" /> object to a <see cref="ContentDTO" /> object.
    /// </summary>
    /// <param name="content">The <see cref="Content" /> object to be mapped.</param>
    /// <returns>The resulting <see cref="ContentDTO" /> object.</returns>
    public ContentDTO Map(Content content) {
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
        if (content is not Post post) return c;
        c.MediaUrl = post?.MediaUrl?.Url;
        c.MediaType = post?.MediaType.ToString();
        c.Thumbnail = null;
        c.Retweets = 0;
        c.Comments = 0; //TODO: Implement this
        return c;
    }
}