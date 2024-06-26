using ObjectMapper.DTO;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Enum;

namespace ObjectMapper.Mappers;

/// <summary>
///   Class responsible for mapping a <see cref="ContentDTO" /> object to a <see cref="Content" /> object.
/// </summary>
public class ContentDomainMapper : IMappingConfig<ContentDTO, Content> {
    CommentDomainMapper _commentDomainMapper;
    PostDomainMapper _postDomainMapper;

    /// <summary>
    ///   Maps a ContentDTO object to a Content object.
    /// </summary>
    public ContentDomainMapper() {
        _commentDomainMapper = new CommentDomainMapper();
        _postDomainMapper = new PostDomainMapper();
    }

    /// <summary>
    ///     Maps a ContentDTO object to a Content object.
    /// </summary>
    /// <param name="input">The ContentDTO object to map.</param>
    /// <returns>The mapped Content object.</returns>
    public Content Map(ContentDTO input) {
        if (input.Type == PostType.Comment.ToString()) {
            return _commentDomainMapper.Map(input);
        }
        return _postDomainMapper.Map(input);
    }
}