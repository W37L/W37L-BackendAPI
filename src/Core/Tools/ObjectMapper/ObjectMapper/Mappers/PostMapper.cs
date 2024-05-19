using ObjectMapper.DTO;

namespace ObjectMapper.Mappers;

/// <summary>
///     The PostMapper class is responsible for mapping instances of the Post class to instances of the ContentDTO class.
/// </summary>
public class PostMapper : IMappingConfig<Post, ContentDTO> {
    
    /// <summary>
    ///     Maps a Post object to a ContentDTO object.
    /// </summary>
    /// <param name="content">The Post object to be mapped.</param>
    /// <returns>The mapped ContentDTO object.</returns>
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